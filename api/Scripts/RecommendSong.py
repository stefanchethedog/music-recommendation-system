import json
import time

import numpy as np
import pandas as pd
import redis
import requests
from redis.commands.search.field import (
    NumericField,
    TagField,
    ListField,
    TextField,
    VectorField,
)
from redis.commands.search.indexDefinition import IndexDefinition, IndexType
from redis.commands.search.query import Query
from sentence_transformers import SentenceTransformer




redis_host = "localhost"
redis_port = 6379
client = redis.Redis(host=redis_host, port=redis_port, decode_responses=True)

keys = client.keys("songs:*")

songs = []

for key in keys:
    song_json = client.get(key)
    song = json.loads(song_json)
    songs.append(song)

res = client.json().get("songs:010", "$.name")

keys = sorted(client.keys("songs:*"))

descriptions = client.json().mget(keys, "$.description")
descriptions = [item for sublist in descriptions for item in sublist]
embedder = SentenceTransformer("msmarco-distilbert-base-v4")
embeddings = embedder.encode(descriptions).astype(np.float32).tolist()
VECTOR_DIMENSION = len(embeddings[0])
# >>> 768

pipeline = client.pipeline()
for key, embedding in zip(keys, embeddings):
    pipeline.json().set(key, "$.description_embeddings", embedding)
pipeline.execute()
# >>> [True, True, True, True, True, True, True, True, True, True, True]

res = client.json().get("songs:010")

schema = (
    ListField("$.genres", no_stem=True, as_name="genres"),
    TextField("$.author", no_stem=True, as_name="author"),
    TextField("$.album", as_name="album"),
    TextField("$.description", as_name="description"),
    VectorField(
        "$.description_embeddings",
        "FLAT",
        {
            "TYPE": "FLOAT32",
            "DIM": VECTOR_DIMENSION,
            "DISTANCE_METRIC": "COSINE",
        },
        as_name="vector",
    ),
)
definition = IndexDefinition(prefix=["songs:"], index_type=IndexType.JSON)
res = client.ft("idx:songs_vss").create_index(
    fields=schema, definition=definition
)
# >>> 'OK'

info = client.ft("idx:songs_vss").info()
num_docs = info["num_docs"]
indexing_failures = info["hash_indexing_failures"]

queries = []
keys = client.keys("search:songs")

songs = []

for key in keys:
    song_json = client.get(key)
    song = json.loads(song_json)
    songs.append(song)

encoded_queries = embedder.encode(queries)


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



query = (
    Query("(*)=>[KNN 3 @vector $query_vector AS vector_score]")
    .sort_by("vector_score")
    .return_fields("vector_score", "id", "name", "author", "genres")
    .dialect(2)
)

create_query_table(query, queries, encoded_queries)
