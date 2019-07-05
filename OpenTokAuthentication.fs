namespace ISchemm.OpenTokFs

open System
open JWT
open JWT.Algorithms
open JWT.Serializers

module OpenTokAuthentication =
    type TokenBody = {
        iss: string
        ist: string
        iat: int64
        exp: int64
        jti: string
    }

    let CreateToken(credentials: IOpenTokCredentials) =
        let payload = {
            iss = credentials.ApiKey.ToString()
            ist = "project"
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            exp = DateTimeOffset.UtcNow.AddMinutes(3.0).ToUnixTimeSeconds()
            jti = Guid.NewGuid().ToString()
        }
        let algorithm = new HMACSHA256Algorithm()
        let serialzier = new JsonNetSerializer()
        let urlEncoder = new JwtBase64UrlEncoder()
        let encoder = new JwtEncoder(algorithm, serialzier, urlEncoder)
        encoder.Encode(payload, credentials.ApiSecret)