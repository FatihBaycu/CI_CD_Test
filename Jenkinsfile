pipeline {
    agent any

    environment {
        IMAGE_NAME = "ci-cd-test"
        CONTAINER_NAME = "ci-cd-test"
        NETWORK_NAME = "public-network"
    }

    stages {

        // =========================
        // 1️⃣ TEST STAGE
        // =========================
        stage('Test') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-u root'
                }
            }
            steps {
                echo 'Running unit tests inside Docker SDK container...'
                sh 'dotnet restore'
                sh 'dotnet test --no-restore'
            }
        }

        // =========================
        // 2️⃣ DOCKER BUILD
        // =========================
        stage('Docker Build') {
            steps {
                echo 'Building Docker image...'
                sh "docker build -t ${IMAGE_NAME}:${BUILD_NUMBER} ."
                sh "docker tag ${IMAGE_NAME}:${BUILD_NUMBER} ${IMAGE_NAME}:latest"
            }
        }

        // =========================
        // 3️⃣ DEPLOY
        // =========================
        stage('Deploy Container') {
            steps {
                echo 'Stopping old container (if exists)...'
                sh "docker stop ${CONTAINER_NAME} || true"
                sh "docker rm ${CONTAINER_NAME} || true"

                echo 'Running new container...'
                sh """
                docker run -d \
                    -p 5000:8080 \
                    --name ${CONTAINER_NAME} \
                    --network ${NETWORK_NAME} \
                    -e ConnectionStrings__PostgreSQL="Host=global-db;Port=5432;Database=ci_cd_test_db;Username=fatih_admin;Password=fatih_pass123" \
                    ${IMAGE_NAME}:${BUILD_NUMBER}
                """
            }
        }
    }

    post {
        success {
            echo "✅ Deployment successful - Build #${BUILD_NUMBER}"
        }
        failure {
            echo "❌ Pipeline failed - Check logs"
        }
    }
}