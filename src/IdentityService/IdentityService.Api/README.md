### Генерация ключа:
1. Генерация приватного ключа
```bash
openssl ecparam -name prime256v1 -genkey -noout -out ecdsa.key
```
2. Создание self-signed сертификата
```bash
openssl req -new -x509 -key ecdsa.key -out ecdsa.crt -days 365
```
3. Создание PFX
```bash
openssl pkcs12 -export -out ecdsa-cert.pfx -inkey ecdsa.key -in ecdsa.crt
```