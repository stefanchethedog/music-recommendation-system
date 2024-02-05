#!./bin/python3
import json
import time
from pprint import pprint

import numpy as np
import pandas as pd
import redis
import requests
from redis.commands.search.field import (
    NumericField,
    TagField,
    TextField,
    VectorField,
)

from redis.commands.search.indexDefinition import IndexDefinition, IndexType
from redis.commands.search.query import Query
from sentence_transformers import SentenceTransformer
import bottle
from bottle import run, route, response, request

bottle.debug(True)
redis_host = "localhost"
redis_port = 6379
client = redis.Redis(host=redis_host, port=redis_port, decode_responses=True)

def create_query_table(query, queries, encoded_queries, extra_params={}):
    results_list = []
    for i, encoded_query in enumerate(encoded_queries):
        result = (
            client.ft("idx:songs_vss")
            .search(
                query,
                {
                    "query_vector": np.array(
                        encoded_query, dtype=np.float32
                    ).tobytes()
                }
                | extra_params,
            )
        )
        result_docs = result.docs
        for doc in result_docs:
            vector_score = round(1 - float(doc.vector_score), 2)
            results_list.append(
                {
                    "query": queries[i],
                    "score": vector_score,
                    "id": doc.id,
                    "name": doc.name,
                    "author": doc.author,
                    "album": doc.album
                }
            )

    return results_list

@route('/zivotinje', method="POST")
def index():
    animals = [
        {
            "name": "zivotinja1",
            "desc": "macka"
        },
        {
            "name": "zivotinja2",
            "desc": "kuce",
        },
        {
            "name": "zivotinja3",
            "desc": "kuce koje laje",
        },
        {
            "name": "zivotinja4",
            "desc": "macka koje laje",
        },
        {
            "name": "zivotinja5",
            "desc": "macka koje mjauce",
        },
        {
            "name": "zivotinja6",
            "desc": "majmun",
        },
    ]
    pipeline = client.pipeline()
    for i, animal in enumerate(animals, start = 1):
        pipeline.json().set(f"animal:{i:03}", "$", animal)
    pipeline.execute()
    embedder = SentenceTransformer("msmarco-distilbert-base-v4")
    keys = sorted(client.keys("animal:*"))
    descriptions = client.json().mget(keys, "$.desc")
    descriptions = [item for sublist in descriptions for item in sublist]
    embeddings = embedder.encode(descriptions).astype(np.float32).tolist()
    VECTOR_DIMENSION = len(embeddings[0])
    pipeline = client.pipeline()
    for key, embedding in zip(keys, embeddings):
        pipeline.json().set(key, "$.desc_embeddings", embedding)
    pipeline.execute()
    #schema = (
    #    TextField("$.name", no_stem=True),
    #    TextField("$.desc"),
    #    VectorField(
    #        "$.desc_embeddings",
    #        "FLAT",
    #        {
    #            "TYPE": "FLOAT32",
    #            "DIM": VECTOR_DIMENSION,
    #            "DISTANCE_METRIC": "COSINE",
    #        },
    #        as_name="vector"
    #    ),
    #)
    #definition = IndexDefinition(prefix=["animal:"], index_type=IndexType.JSON)
    #res = client.ft("idx:animals_vss").create_index(
    #    fields=schema, definition=definition
    #)
    #print("Index information")
    #info = client.ft("idx:animals_vss").info()
    #num_docs = info["num_docs"]
    #indexing_failures = info["hash_indexing_failures"]
    #print(f"docs: {num_docs} failures: {indexing_failures}")

    #keys = client.keys("animal:*")
    #redis_songs = []
    #for key in keys:
    #    song = client.json().get(key)
    #    redis_songs.append(song)

    queries = ['macka']
    encoded_queries = embedder.encode(queries).astype(np.float32).tolist()
    query = (
        Query('(*)=>[KNN 3 @vector $query_vector AS vector_score]')
         .sort_by('vector_score')
         .return_fields('vector_score', '$.name')
         .dialect(2)
    )
    results = create_query_table(query, queries, encoded_queries)
    return results

@route('/recommendations', method="POST")
def index():
    req = request.json
    songs = req['songs']
    queries = req['queries']
    pass
    for song in songs:
        description = f"{song['Name']} {song['Author']} {song['Album']} {' '.join(song['Genres'])}"
        song["Description"] = description
    pipeline = client.pipeline()
    for i, song in enumerate(songs, start = 1):
        key = f"song:{i:03}"
        pipeline.json().set(key, '$', song)
    res = pipeline.execute()
    embedder = SentenceTransformer("msmarco-distilbert-base-v4")
    keys = sorted(client.keys("song:*"))
    descriptions = client.json().mget(keys, "$.Description")
    descriptions = [item for sublist in descriptions for item in sublist]
    embeddings = embedder.encode(descriptions).astype(np.float32).tolist()
    VECTOR_DIMENSION = len(embeddings[0])
    pipeline = client.pipeline()
    for key, embedding in zip(keys, embeddings):
        pipeline.json().set(key, "$.Description_embeddings", embedding)
    pipeline.execute()
    #NOTE: the following code block should be run only once at the database startup ( NOT THE SCRIPT STARTUP )

    #schema = (
    #    TextField("$.Name", no_stem=True, as_name="name"),
    #    TextField("$.Description", as_name="description"),
    #    TextField("$.Album", as_name="album"),
    #    TextField("$.Author", as_name="author"),
    #    VectorField(
    #        "$.Description_embeddings",
    #        "FLAT",
    #        {
    #            "TYPE": "FLOAT32",
    #            "DIM": VECTOR_DIMENSION,
    #            "DISTANCE_METRIC": "COSINE",
    #        },
    #        as_name="vector"
    #    ),
    #)
    #definition = IndexDefinition(prefix=["song:"], index_type=IndexType.JSON)
    #res = client.ft("idx:songs_vss").create_index(
    #    fields=schema, definition=definition
    #)
    #print("Index information")
    #info = client.ft("idx:songs_vss").info()
    #num_docs = info["num_docs"]
    #indexing_failures = info["hash_indexing_failures"]
    #print(f"docs: {num_docs} failures: {indexing_failures}")

    keys = client.keys("song:*")
    redis_songs = []
    for key in keys:
        song = client.json().get(key)
        redis_songs.append(song)
    encoded_queries = embedder.encode(queries).astype(np.float32).tolist()
    query = (
        Query('(*)=>[KNN 3 @vector $query_vector as vector_score]')
        .sort_by('vector_score')
         .return_fields('vector_score', 'name', 'description', 'album', 'author')
         .dialect(2)
    )
    results = create_query_table(query, queries, encoded_queries)
    result = json.dumps(results)
    print(result)
    return result

run(host='localhost', port=6666)
