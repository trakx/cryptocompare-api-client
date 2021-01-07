## Avoid committing you secrets and keys 
In order to be able to run some integration tests, you should create a .env file with the following items based on the Configuration classes you instanciate: 

### Trakx.CryptoCompare.ApiClient.CryptoCompareApiConfiguration

	CryptoCompareApiConfiguration__ApiKey

### Complete .env file sample

	CryptoCompareApiConfiguration__ApiKey

### You should update the path to your .env file in src/Trakx.Tests/Tools/Secrets.cs
