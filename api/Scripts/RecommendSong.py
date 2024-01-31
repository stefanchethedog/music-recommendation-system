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
        result_docs = (
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
            .docs
        )
        print("result docs")
        pprint(result_docs)
        for doc in result_docs:
            vector_score = round(1 - float(doc.vector_score), 2)
            results_list.append(
                {
                    "query": queries[i],
                    "score": vector_score,
                    "id": doc.id,
                    "name": doc.name,
                    "album": doc.album,
                    "author": doc.author,
                    "genres": doc.genres,
                }
            )

    return results_list


@route('/recommendations', method="POST")
def index():
    songs = request.json
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
    #    TextField("$.Name", no_stem=True, as_name="Name"),
    #    TagField("$.Author", as_name="Author"),
    #    TagField("$.Genres", as_name="Genres"),
    #    TextField("$.Description", as_name="Description"),
    #    VectorField(
    #        "$.description_embeddings",
    #        "FLAT",
    #        {
    #            "TYPE": "FLOAT32",
    #            "DIM": VECTOR_DIMENSION,
    #            "DISTANCE_METRIC": "COSINE",
    #        },
    #        as_name="vector",
    #    ),
    #)
    #definition = IndexDefinition(prefix=["song:"], index_type=IndexType.JSON)
    #res = client.ft("idx:songs_vss").create_index(
    #    fields=schema, definition=definition
    #)
    #info = client.ft("idx:songs_vss").info()
    #num_docs = info["num_docs"]
    #indexing_failures = info["hash_indexing_failures"]
    #print(f"docs: {num_docs} failures: {indexing_failures}")
    queries = list(map(lambda song: f"{song['Name']} {song['Author']} {song['Album']} {' '.join(song['Genres'])}", songs))
    encoded_queries = embedder.encode(queries)
    query = (
        Query('(*)=>[KNN 3 @vector $query_vector AS vector_score]')
         .sort_by('vector_score')
         .return_fields('vector_score', 'Name', 'Author', 'Genres', 'Description', 'Album')
         .dialect(2)
    )
    results = create_query_table(query, queries, encoded_queries)
    pprint(results)
    return results
#def index():
#    keys = client.keys("songs:*")
#
#    songs = []
#    for key in keys:
#        song_json = client.get(key)
#        song = json.loads(song_json)
#        songs.append(song)
#
#    keys = sorted(client.keys("songs:*"))
#    pprint(songs)
#
#    descriptions = client.json().mget(keys, "$.description")
#    descriptions = [item for sublist in descriptions for item in sublist]
#    embedder = SentenceTransformer("msmarco-distilbert-base-v4")
#    embeddings = embedder.encode(descriptions).astype(np.float32).tolist()
#    VECTOR_DIMENSION = len(embeddings[0])
#    # >>> 768
#
#    pipeline = client.pipeline()
#    for key, embedding in zip(keys, embeddings):
#        pipeline.json().set(key, "$.description_embeddings", embedding)
#    pipeline.execute()
#
#    res = client.json().get("songs:010")
#
#    schema = (
#        VectorField("$.genres", no_stem=True, as_name="genres"),
#        TextField("$.author", no_stem=True, as_name="author"),
#        TextField("$.album", as_name="album"),
#        TextField("$.description", as_name="description"),
#        VectorField(
#            "$.description_embeddings",
#            "FLAT",
#            {
#                "TYPE": "FLOAT32",
#                "DIM": VECTOR_DIMENSION,
#                "DISTANCE_METRIC": "COSINE",
#            },
#            as_name="vector",
#        ),
#    )
#    definition = IndexDefinition(prefix=["songs:"], index_type=IndexType.JSON)
#    res = client.ft("idx:songs_vss").create_index(
#        fields=schema, definition=definition
#    )
#    # >>> 'OK'
#
#    info = client.ft("idx:songs_vss").info()
#    num_docs = info["num_docs"]
#    indexing_failures = info["hash_indexing_failures"]
#
#    queries = []
#    keys = client.keys("search:songs")
#
#    songs = []
#
#    for key in keys:
#        song_json = client.get(key)
#        song = json.loads(song_json)
#        songs.append(song)
#
#    encoded_queries = embedder.encode(songs)
#    query = (
#        Query("(*)=>[KNN 3 @vector $query_vector AS vector_score]")
#        .sort_by("vector_score")
#        .return_fields("vector_score", "id", "name", "author", "genres")
#        .dialect(2)
#    )
#    response.content_type = "application/json"
#    res = create_query_table(query, songs, encoded_queries)
#    pprint(res)
#    return json.dumps(res)
#
run(host='localhost', port=6666)
