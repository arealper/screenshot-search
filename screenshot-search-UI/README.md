# Screenshot Search App

This project explores how semantic search can be integrated into applications with minimal cost and infrastructure.
It enables screenshot uploads, text extraction via OCR, and meaning-based search rather than relying solely on exact keyword matching.
For example, a search for "invoice" will also return results for "receipt".

The implementation demonstrates that affordable or free AI endpoints can be used to generate embeddings, vectors can be stored locally in SQLite, and vector similarity searches can be performed without dedicated vector databases or costly services — making semantic search accessible for a wide range of projects.

---

## Features
- Upload screenshots via a clean web UI
- Search by semantic meaning (vector embeddings)
- OCR text extraction from screenshots
- Fast similarity search using SQLite
- Built with Angular + Angular Material for a modern UI
- C# .NET backend with AI integration

---

## Tech Stack

**Frontend:**
- Angular 19
- Angular Material

**Backend:**
- ASP.NET Core Web API
- Cloudflare Workers AI REST API for embeddings
- Entity Framework with SQLite (vector search)
- OCR via [Tesseract](https://github.com/tesseract-ocr/tesseract)

---

## Project Structure
/screenshot-search-API  
/screenshot-search-UI

## Installation

### 1️⃣ Clone the repo
git clone https://github.com/arealper/screenshot-search.git  
cd screenshot-search


### 2️⃣ Backend setup
cd screenshot-search-API  
dotnet restore  
dotnet ef database update  
dotnet run  

> **Note**  
> This project uses **SQLite** as the database, stored locally in the `data` folder.  
> No external database or paid vector database services are required — all data and embeddings are stored locally.  
> However, you will need a **free Cloudflare account** to obtain API credentials for generating embeddings.


### 3️⃣ Frontend setup
cd /screenshot-search-UI  
npm install  
ng serve

## How it Works
Upload a screenshot → backend saves the file and extracts text with OCR.  
Generate Embeddings → Uses Cloudflare AI endpoints to transform text into numerical vector embeddings for semantic search.  
Store in SQLite → vectors and metadata are stored locally.  
Search → user query is embedded and compared via cosine similarity to find the most relevant screenshots.
