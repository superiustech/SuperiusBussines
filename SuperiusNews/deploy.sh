#!/bin/bash

KEY_PATH="C:/Users/Ibid/Documents/LINUX-3_key.pem"
USER="azureuser"
HOST="20.0.114.230"
REMOTE_DIR="/var/www/superiusnews"
REMOTE_PUBLISH_DIR="$REMOTE_DIR/publish"
PROJECT_NAME="SuperiusNews"
LOCAL_PUBLISH_DIR="./publish"

echo "Construindo frontend React..."
cd ClientApp
npm install
npm run build
cd ..

echo "Preparando estrutura de diretórios local..."
# Garante que temos permissões para manipular a pasta publish
if [ -d "$LOCAL_PUBLISH_DIR" ]; then
  sudo chown -R $USER:$USER $LOCAL_PUBLISH_DIR
  rm -rf $LOCAL_PUBLISH_DIR
fi

mkdir -p $LOCAL_PUBLISH_DIR/ClientApp

echo "Copiando build do React para dentro da pasta publish..."
cp -r ClientApp/build $LOCAL_PUBLISH_DIR/ClientApp/build

echo "Publicando backend .NET..."
dotnet publish "$PROJECT_NAME.csproj" -c Release -o $LOCAL_PUBLISH_DIR

echo "Configurando diretório remoto..."
ssh -i "$KEY_PATH" "$USER@$HOST" << EOF
  sudo mkdir -p $REMOTE_DIR
  sudo chown -R $USER:$USER $REMOTE_DIR
  if [ -d "$REMOTE_PUBLISH_DIR" ]; then
    rm -rf $REMOTE_PUBLISH_DIR
  fi
  mkdir -p $REMOTE_PUBLISH_DIR
EOF

echo "Enviando arquivos para o servidor Linux..."
scp -i "$KEY_PATH" -r $LOCAL_PUBLISH_DIR/* "$USER@$HOST:$REMOTE_PUBLISH_DIR"

echo "Deploy concluído com sucesso!"