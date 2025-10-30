# Estado del Arte I.A.

**Autor:** Víctor Manuel Ridao Chaves  
**Asignatura:** IA & Ciberseguridad  
**Fecha:** 20/10/2025  

---

## 📑 Índice

1. [¿Qué es la Inteligencia Artificial?](#qué-es-la-inteligencia-artificial)  
2. [I.A. Generativa de Textos](#ia-generativa-de-textos)  
   - [Herramientas destacadas](#herramientas-destacadas-1)  
   - [Prueba a realizar](#prueba-a-realizar-1)  
3. [I.A. Generativa de Imágenes](#ia-generativa-de-imágenes)  
   - [Herramientas destacadas](#herramientas-destacadas-2)  
   - [Prueba a realizar](#prueba-a-realizar-2)  
4. [I.A. Generativa para Desarrolladores](#ia-generativa-para-desarrolladores)  
   - [Herramientas destacadas](#herramientas-destacadas-3)  
   - [Prueba a realizar](#prueba-a-realizar-3)  
5. [Comentario final](#comentario-final)

---

## 🤖 ¿Qué es la Inteligencia Artificial?

La Inteligencia Artificial (IA) se puede definir de muchas maneras, pero principalmente se la describe como un **sistema de software y hardware diseñado por humanos** capaz de actuar en la dimensión física o digital.  
Es capaz de percibir su entorno gracias a la adquisición e interpretación de **datos estructurados y no estructurados**.  
Puede razonar sobre el conocimiento, procesar la información derivada de estos datos y decidir las mejores acciones para lograr un objetivo dado.

En resumen, la IA combina técnicas de aprendizaje automático, redes neuronales y procesamiento de datos para imitar ciertas capacidades humanas, como la percepción, la planificación o la toma de decisiones.

---

## 🧠 I.A. Generativa de Textos

Este tipo de Inteligencia Artificial ayuda a crear todo tipo de textos, desde publicaciones en redes sociales hasta artículos completos o fragmentos de código.  
Su utilidad radica en la **facilidad con la que puede generar contenido coherente, informativo y adaptado al contexto**.  

Hoy en día, gracias al auge de las inteligencias artificiales, tenemos muchas opciones a la hora de utilizar una I.A. generativa de textos. A continuación, se presentan las más influyentes.

---

### ⚙️ Herramientas destacadas

| Herramienta | Descripción | Uso principal | Acceso |
|--------------|-------------|----------------|---------|
| **ChatGPT** | Capaz de mantener conversaciones naturales, generar código y redactar textos. | Redacción, programación, aprendizaje. | Gratuito y de pago |
| **Gemini** | Ofrece respuestas contextuales e integración con Gmail, Drive y YouTube. | Asistente de productividad y análisis. | Gratuito con cuenta Google |
| **Claude** | Modelo centrado en la seguridad y la comprensión de textos. Ideal para análisis, redacción y tareas creativas. | Lectura de documentos, escritura avanzada, resumen. | Gratuito y versiones Pro |

---

### 🧪 Prueba a realizar

**Prueba:** Se le pidió a ChatGPT que generase un fragmento de código JavaScript que calcule la media de una lista de números.

```javascript
function calcularMedia(numeros) {
  let suma = 0;
  for (let num of numeros) {
    suma += num;
  }
  return suma / numeros.length;
}

## 🎨 I.A. Generativa de Imágenes

Los generadores de imágenes con I.A., o **modelos generativos**, son sistemas entrenados para crear nuevas imágenes de forma automatizada según los datos solicitados por el usuario.  
Estas herramientas permiten convertir texto, fotografías o bocetos en imágenes creadas por IA, aplicándose en **arte, diseño, moda, videojuegos, ciencia**, entre otros campos.

Su funcionamiento se basa en el uso de **redes neuronales generativas adversarias (GANs)**.  
El generador crea imágenes con datos aleatorios y el discriminador evalúa si son reales o no.  
Ambos compiten, provocando que el generador se vuelva cada vez más hábil creando imágenes más realistas.

El resultado son imágenes tan detalladas que, a menudo, es difícil reconocer si son auténticas o no.  
La calidad final dependerá del modelo utilizado y del nivel de detalle del *prompt* del usuario.

---

### ⚙️ Herramientas destacadas

| Herramienta | Descripción | Uso principal | Acceso |
|--------------|-------------|----------------|---------|
| **DALL·E** | Genera imágenes a partir de texto y permite editar partes específicas. | Creación de ilustraciones y diseños personalizados. | Gratuito con límite / Plus |
| **Midjourney** | Muy popular entre artistas digitales. Ofrece resultados de alta calidad con estilo artístico y realista. | Arte conceptual, retratos y paisajes. | Suscripción mensual |
| **Leonardo.ai** | Plataforma con enfoque profesional en diseño, *concept art* y assets para videojuegos. | Creación de imágenes y texturas para proyectos creativos. | Registro gratuito / versión Pro |

---

### 🧪 Prueba a realizar

**Prueba:** Se utilizó **DALL·E** para generar una imagen de un *Big Daddy* de la saga **Bioshock**.

**Resultado:**  
El modelo generó una figura humanoide con un estilo cercano, pero no idéntico al diseño original.  
Fue necesario refinar el *prompt* para acercar el resultado al modelo deseado.

**Conclusión:**  
El resultado no fue completamente acertado, pero demuestra el potencial de estas herramientas para generar imágenes creativas a partir de descripciones simples.  
La calidad final depende en gran medida de la claridad del *prompt* y del modelo utilizado.

## 💻 I.A. Generativa para Desarrolladores

La I.A. para programadores se centra en herramientas diseñadas para **asistir en la escritura, depuración y optimización de código**.  
Estas inteligencias artificiales utilizan modelos entrenados con repositorios de código (como GitHub o Stack Overflow) y son capaces de **predecir líneas de código, explicar funciones, detectar errores** o incluso **generar programas completos** a partir de una descripción textual.

Se han convertido en un apoyo habitual para desarrolladores, tanto principiantes como profesionales, reduciendo el tiempo de desarrollo y mejorando la calidad del software.

---

### ⚙️ Herramientas destacadas

| Herramienta | Descripción | Uso principal | Acceso |
|--------------|-------------|----------------|---------|
| **GitHub Copilot** | Desarrollado por GitHub y OpenAI. Sugiere código en tiempo real dentro de editores como Visual Studio Code o JetBrains. | Autocompletar código, generar funciones y documentación. | Suscripción mensual (prueba gratuita) |
| **Codeium** | Alternativa gratuita a Copilot. Ofrece sugerencias de código, chat integrado y soporte para más de 70 lenguajes. | Asistente de código y depuración. | Gratuito / versión Pro |
| **Tabnine** | IA enfocada en privacidad y velocidad. Aprende del estilo del propio desarrollador. | Autocompletado y predicción de código. | Gratuito / pago por equipos |

---

### 🧪 Prueba a realizar

Se probó **Codeium** en **Visual Studio Code** con un archivo JavaScript.

**Prueba realizada:**  
Se escribió el comentario:
```javascript
// función que calcule si un número es primo

function esPrimo(numero) {
  if (numero <= 1) return false;
  for (let i = 2; i < numero; i++) {
    if (numero % i === 0) return false;
  }
  return true;
}


console.log(calcularMedia([5, 7, 9, 10])); // Resultado: 7.75
