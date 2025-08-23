# Makefile for dotnet-minimal — basic development tasks

SOLUTION ?= minimal.sln
PROJECT ?= minimal.csproj
CONFIG ?= Debug
DOTNET ?= dotnet
IMAGE_REF ?= ghcr.io/benc-uk/dotnet-minimal:latest	

.PHONY: help all restore build run watch clean lint lint-fix install-tools

help: ## Show this help
	@awk 'BEGIN {FS = ":.*?## "}; /^[a-zA-Z0-9._-]+:.*##/ { printf "  %-15s %s\n", $$1, $$2 }' $(MAKEFILE_LIST)

all: build ## Build the project (default)

restore: ## Restore NuGet packages for the solution
	$(DOTNET) restore $(SOLUTION)

build: restore ## Build the solution
	$(DOTNET) build $(SOLUTION) -c $(CONFIG) --no-restore

publish: ## Publish the project as a self-contained app
	$(DOTNET) publish $(PROJECT) -c $(CONFIG) -o dist/

run: ## Run the application
	$(DOTNET) run --project $(PROJECT) --configuration $(CONFIG)

watch: ## Run the app with dotnet watch
	$(DOTNET) watch --project $(PROJECT) run --configuration $(CONFIG)

clean: ## Clean build artifacts
	$(DOTNET) clean $(SOLUTION)
	rm -rf bin/ obj/ dist/

lint: ## Verify & lint code 
	@echo "Running analyzer & format check..."
	dotnet format --verify-no-changes $(SOLUTION)

lint-fix: ## Fix code format issues
	@echo "Formatting solution..."
	dotnet format $(SOLUTION)

image: ## Build container image
	docker build -t $(IMAGE_REF) .

push: image ## Push container image to registry
	docker push $(IMAGE_REF)