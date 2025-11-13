// index.js
import express from "express";
import dotenv from "dotenv";
import { streamText } from "ai";
import { createOpenAI } from "@ai-sdk/openai";

dotenv.config();

const app = express();
app.use(express.json()); // Para recibir JSON en POST

// Configura el cliente OpenAI con tu clave
const openai = createOpenAI({
    apiKey: process.env.OPENAI_API_KEY,
});

// Ruta POST /chat
app.post("/chat", async (req, res) => {
    try {
        const { prompt } = req.body;
        if (!prompt) return res.status(400).json({ error: "Falta el prompt" });

        // Genera texto con prompt dinámico
        const { textStream } = await streamText({
            model: openai("gpt-4o-mini"),
            prompt, // tu prompt dinámico desde Postman o curl
        });

        let result = "";
        for await (const chunk of textStream) result += chunk;

        // Devuelve la respuesta
        res.json({ respuesta: result });
    } catch (error) {
        console.error("Error generando texto:", error);
        res.status(500).json({ error: error.message });
    }
});

// Ruta GET para probar que el servidor funciona
app.get("/", (req, res) => {
    res.send("<h1>Servidor de IA activo. Usa POST /chat para enviar prompts.</h1>");
});

// Inicia el servidor
const PORT = 3000;
app.listen(PORT, () => {
    console.log(`✅ Servidor de IA activo en http://localhost:${PORT}`);
});
