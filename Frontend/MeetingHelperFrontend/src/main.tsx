// src/main.jsx
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.jsx'

// Hittar <div id="root"> i index.html och trycker in din App-komponent där
createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)