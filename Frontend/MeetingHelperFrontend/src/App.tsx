import { BrowserRouter, Link } from 'react-router-dom';
import AppRouter from './router/AppRouter';

export default function App() { 
  // BrowserRouter has to enclose everything that has to do with routing
  // It makes Link able to change the URL and Route able to look at the URL
  return(
    <BrowserRouter>
      <header>
        {/* Create a nav bar with links that changes the URL address. */}
        <nav style={{ display: 'flex', gap: '1rem', padding: '1rem' }}>
          <Link to="/">Home</Link>
        </nav>
      </header>
      <main>
        <AppRouter /> {/* This is where the new page will show up. Everything else is static */}
      </main>
    </BrowserRouter>
  );
}