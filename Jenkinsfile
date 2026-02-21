pipeline {
    agent any

    environment {
        IMAGE_NAME = "ci-cd-test"
    }

    stages {

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
                docker run -d -p 5000:8080 --name ${IMAGE_NAME} ${IMAGE_NAME}:${BUILD_NUMBER}
                """
            }
        }
    }
}