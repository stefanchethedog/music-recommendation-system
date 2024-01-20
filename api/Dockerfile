# Use the official Neo4j image as the base image
FROM neo4j:latest

# Set environment variables
ENV NEO4J_AUTH=none \
    NEO4J_dbms_memory_pagecache_size=1G \
    NEO4J_dbms.memory.heap.max_size=2G

# Expose the ports used by Neo4j
EXPOSE 7474 7473 7687

# (Optional) Add any additional configurations or setup steps here

# The command to run when the container starts
CMD ["neo4j"]
