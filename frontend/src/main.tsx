import React from 'react';
import ReactDOM from 'react-dom/client';
import Routes from './Routes';
import './styles/global.scss';
import { QueryClientProvider } from 'react-query';
import { ReactQueryDevtools } from 'react-query/devtools';
import { queryClient } from './services/queryClient';
import { Toaster } from 'react-hot-toast';

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <Toaster position="top-center" reverseOrder={true} />
      <Routes />
      {import.meta.env.VITE_ReactQueryDevtools_OPEN == 'true' && (
        <ReactQueryDevtools initialIsOpen={false} />
      )}
    </QueryClientProvider>
  </React.StrictMode>
);
