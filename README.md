# Acme RabbitMQ

``` mermaid
flowchart LR
    P[Publisher API]
    R[RabbitMQ]
    C[Consumer API]
    P --[Publish messages]--> R
    R --[Consume recent messages]-->C
```

## Development

``` bash
git clone https://github.com/teerapongt/acme-rabbitmq.git
cd acme-rabbitmq
```

``` bash
docker compose up -d --build
```

## Production

``` bash
docker compose -f compose.yml -f compose.prod.yml up -d --build
```

## Urls
| Component           | Url                     | Note                             |
|---------------------|-------------------------|----------------------------------|
| Publisher API       | http://localhost:5001   |                                  |
| Consumer API        | http://localhost:5002   |                                  |
| RabbitMQ Management | http://localhost:15672  | Username: guest, Password: guest |
