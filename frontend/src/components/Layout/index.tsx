import Header from './Header';
import Footer from './Footer';

import { Outlet } from 'react-router-dom';
import { DataProvider } from '../../context/DataProvider';

export default function Layout() {
  return (
    <DataProvider>
      <div className="flex flex-col theme">
        <Header />
        <main className="flex-grow">
          <Outlet />
        </main>
        <Footer />
      </div>
    </DataProvider>
  );
}
