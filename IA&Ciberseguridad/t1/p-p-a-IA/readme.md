# 📝 Enunciado del Ejercicio: Piedra, Papel o Tijeras con Análisis Predictivo

Este documento describe la mecánica y los esquemas de datos para un ejercicio automatizado de **Piedra, Papel o Tijeras** donde la **Máquina (IA)** actúa como un agente analítico, intentando predecir el patrón de juego del **Jugador (Usuario)**.

## 1\. ⚙️ Reglas y Mecánica del Juego

1.  **Iteración:** El juego avanza por turnos. El Jugador envía el estado del juego y la Máquina responde con su jugada y su análisis.
2.  **Cálculo:** El Jugador es responsable de calcular el resultado de cada ronda (victoria, derrota, o empate), actualizar la puntuación e incorporar ambas jugadas al historial (`history`).
3.  **Análisis:** La Máquina utiliza el `history.player` para calcular la **previsibilidad** y predecir el siguiente movimiento del Jugador.
4.  **Finalización:** La partida concluye cuando el Jugador indica que el campo `"finish"` es `true`.

-----

## 2\. 📤 Esquema de Datos de Envío (Jugador $\to$ Máquina)

El Jugador debe enviar un objeto JSON en **cada turno** con el estado completo y actualizado de la partida.

| Campo                 | Tipo              | Valores Posibles                 | Descripción                                  |
| :-------------------- | :---------------- | :------------------------------- | :------------------------------------------- |
| **`score`**           | Objeto            | N/A                              | Contenedor de la puntuación actual.          |
| **`score.player`**    | Número            | Entero $\ge 0$                   | Puntuación total del Jugador.                |
| **`score.machine`**   | Número            | Entero $\ge 0$                   | Puntuación total de la Máquina.              |
| **`history`**         | Objeto            | N/A                              | Contenedor del historial de jugadas.         |
| **`history.player`**  | Array de `string` | `["piedra", "papel", "tijeras"]` | Secuencia de jugadas pasadas del Jugador.    |
| **`history.machine`** | Array de `string` | `["piedra", "papel", "tijeras"]` | Secuencia de jugadas pasadas de la Máquina.  |
| **`finish`**          | Booleano          | `true` / `false`                 | Indica si la partida ha finalizado (`true`). |

### Ejemplo de JSON de Envío (Ronda N):

```json
{
  "score": {
    "player": 2,
    "machine": 3
  },
  "history": {
    "player": ["piedra", "papel", "tijeras", "tijeras", "piedra"],
    "machine": ["papel", "papel", "tijeras", "papel", "piedra"]
  },
  "finish": false
}
```

-----

## 3\. 📥 Esquema de Datos de Respuesta (Máquina $\to$ Jugador)

La Máquina debe responder **exclusivamente** con un objeto JSON.

| Campo                                      | Tipo   | Valores Posibles                   | Descripción                                                                      |
| :----------------------------------------- | :----- | :--------------------------------- | :------------------------------------------------------------------------------- |
| **`next_move`**                            | String | `"piedra"`, `"papel"`, `"tijeras"` | Jugada de la Máquina para la **ronda actual**.                                   |
| **`analysis`**                             | Objeto | N/A                                | Contenedor de la métrica y predicción.                                           |
| **`analysis.predictability_percentage`**   | Número | `0.0` a `100.0`                    | Porcentaje de aciertos de predicción de la Máquina sobre el Jugador.             |
| **`analysis.player_next_move_prediction`** | String | `"piedra"`, `"papel"`, `"tijeras"` | Jugada que la Máquina **predice** que el Jugador hará en la **siguiente ronda**. |

### Ejemplo de JSON de Respuesta (Ronda N):

```json
{
  "next_move": "papel",
  "analysis": {
    "predictability_percentage": 66.7,
    "player_next_move_prediction": "piedra" 
  }
}
```

-----

## 4\. 🛑 Condición de Finalización

Si el Jugador envía un JSON con el campo `"finish": true`, la Máquina **debe ignorar el formato JSON de respuesta** y devolver un **análisis exhaustivo en texto plano**.

El análisis final debe detallar:

1.  El **patrón principal** de juego detectado en el Jugador.
2.  Las **desviaciones clave** o rupturas del patrón.
3.  El **porcentaje final de previsibilidad** de sus jugadas.
