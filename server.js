// server.js
const express = require('express');
const cors = require('cors');
const ConvertorioClient = require('convertorio-sdk');

const app = express();
app.use(cors());
app.use(express.json());

// Вставьте сюда ваш код (но с реальным ключом)
const client = new ConvertorioClient({ apiKey: 'ВАШ_РЕАЛЬНЫЙ_КЛЮЧ' });

app.post('/convert', async (req, res) => {
  try {
    const result = await client.convertFile({
      inputPath: './uploads/image.png', // путь на сервере, а не в браузере
      targetFormat: 'jpg'
    });
    res.json({ success: true, output: result.outputPath });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

app.listen(3001, () => console.log('Сервер запущен на порту 3001'));