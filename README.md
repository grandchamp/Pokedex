# Pay by Bank Challenge

## Technology

- .NET Core 5
- ASP.NET Core MVC
- Docker

## Architectural idea
The idea here was to create a `PokemonService` who is the main responsible for applying the business requirements for the domain.

This service is open for extension and closed for modification by applying composition with `PokemonTransformationActions`.

If in a future is needed to add more transformations to a Pokemon, it's possible to just add a new action into `PokemonTransformationActions` and this service code will not be changed.

It's also possible to combine multiple actions (the enum has `[Flag]` decorator).

This service will fetch data from an adapter `IPokemonDataAdapter` that is currently consuming `PokeApi` client and apply all transformations registered in the DI.

Both API Clients (`PokeApi` and `FunTranslations`) are very very simple, just returning the minimum amount of info.

## Future improvements
Possibly adding `Polly` to handle errors, retry unsuccessful calls, etc.

## Production API

Add support to HTTPS or host it behind a load balancer that has HTTPS.

## Usage

```bash
cd Pokedex
docker build -f Pokedex.Api/Dockerfile -t pokedex-api .
docker run -d -e "ASPNETCORE_URLS=http://+:80" -e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true" -e "ASPNETCORE_ENVIRONMENT=Development" -P -t pokedex-api
```

After running `docker run` please run
```bash
docker ps
```

You'll need to check what port is bound to the instance.
```bash
 0.0.0.0:49178->80/tcp
```

In this case is `49178`.

Then, open chrome with URL `http://localhost:49178/swagger` and you'll see Swagger/OpenAPI endpoint.