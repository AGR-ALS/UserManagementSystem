# Setup

## Clone Repository
```
git clone https://github.com/AGR-ALS/UserManagementSystem.git
cd task4_repo/
```

## Enter enviromental variables

Create `.env` files and write variables into them according to `.env_examples` files. 

For example:
```
EmailSettings__FromName='App'
EmailSettings__FromEmail='test@test.com'
EmailSettings__ToName='Recipient'
EmailSettings__ClientHost='smtp.gmail.com'
EmailSettings__ClientPort=465
EmailSettings__ClientLogin='test@test.com'
EmailSettings__ClientPassword='qwerty'
EmailSettings__UseSsl=true

RabbitMqSettings__Host=rabbitmq://rabbitmq
RabbitMqSettings__Username=test
RabbitMqSettings__Password=test
```

## Launch the App

```
docker compose up --build -d
```

### You can access the app at [localhost:5085](http://localhost:5085)
