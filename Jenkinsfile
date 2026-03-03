pipeline {
    agent any

    environment {
        IMAGE_NAME = "ci-cd-test"
    }

    stages {
        stage('Test') {
            steps {
                echo 'Testing with Docker SDK image...'
                // Jenkins içinde dotnet olmadığı için dışarıdan SDK imajı çağırıyoruz
                sh "docker run --rm -v ${WORKSPACE}:/app -w /app mcr.microsoft.com/dotnet/sdk:8.0 dotnet test CI_CD_Test.sln"
            }
        }

        stage('Docker Build') {
            steps {
                sh "docker build -t ${IMAGE_NAME}:${BUILD_NUMBER} ."
                sh "docker tag ${IMAGE_NAME}:${BUILD_NUMBER} ${IMAGE_NAME}:latest"
            }
        }

        stage('Deploy Container') {
            steps {
                sh """
                docker stop ${IMAGE_NAME} || true
                docker rm ${IMAGE_NAME} || true
                docker run -d \
                    -p 5000:8080 \
                    --name ${IMAGE_NAME} \
                    --network public-network \
                    -e ConnectionStrings__PostgreSQL="Host=global-db;Port=5432;Database=ci_cd_test_db;Username=fatih_admin;Password=fatih_pass123" \
                    ${IMAGE_NAME}:${BUILD_NUMBER}
                """
            }
        }
    }
}