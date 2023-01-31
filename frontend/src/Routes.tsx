import {
  createBrowserRouter,
  RouteObject,
  RouterProvider,
} from 'react-router-dom';

import PrivateRoute from './PrivateRoute';

import Layout from './components/Layout';

import { Home } from './pages/Home';

import { Orders } from './pages/Orders';

import Registration from './pages/Registration';

import NotFound from './pages/404';

import Dashboard from './pages/Dashboard/Dashboard';
import { Components } from './pages/Dashboard/Components';
import { Models } from './pages/Dashboard/Models';
import { Providers } from './pages/Dashboard/Providers';
import { Tracking } from './pages/Dashboard/Tracking';
import { Workers } from './pages/Dashboard/Workers';
import { ProductionProcesses } from './pages/Dashboard/ProductionProcesses';
import Users from './pages/Dashboard/Users';
import Organizations from './pages/Dashboard/Organizations';
import { Organization } from './pages/Dashboard/Organization';
import { Clients } from './pages/Dashboard/Clients';
import { Settings } from './pages/Settings';

const routesDashboard: RouteObject[] = [
  {
    index: true,
    element: (
      <PrivateRoute
        path="/dashboard"
        childrenSuccess={<Tracking />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'models',
    element: (
      <PrivateRoute
        path="/dashboard/models"
        childrenSuccess={<Models />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'production_processes',
    element: (
      <PrivateRoute
        path="/dashboard/production_processes"
        childrenSuccess={<ProductionProcesses />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'providers',
    element: (
      <PrivateRoute
        path="/dashboard/providers"
        childrenSuccess={<Providers />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'components',
    element: (
      <PrivateRoute
        path="/dashboard/components"
        childrenSuccess={<Components />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'workers',
    element: (
      <PrivateRoute
        path="/dashboard/workers"
        childrenSuccess={<Workers />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'users',
    element: (
      <PrivateRoute
        path="/dashboard/users"
        childrenSuccess={<Users />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'organization',
    element: (
      <PrivateRoute
        path="/dashboard/clients"
        childrenSuccess={<Organization />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'organizations',
    element: (
      <PrivateRoute
        path="/dashboard/clients"
        childrenSuccess={<Organizations />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'clients',
    element: (
      <PrivateRoute
        path="/dashboard/clients"
        childrenSuccess={<Clients />}
        pathError="/40"
      />
    ),
  },
];

const routes: RouteObject[] = [
  {
    index: true,
    element: (
      <PrivateRoute
        path="/"
        childrenSuccess={<Home />}
        pathError="/registration"
      />
    ),
  },
  {
    path: 'orders',
    element: (
      <PrivateRoute
        path="/orders"
        childrenSuccess={<Orders />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'registration',
    element: (
      <PrivateRoute
        path="/registration"
        childrenSuccess={<Registration />}
        pathError="/404"
      />
    ),
  },
  {
    path: 'dashboard',
    element: <Dashboard />,
    children: routesDashboard,
  },
  {
    path: 'settings',
    element: (
      <PrivateRoute
        path="/settings"
        childrenSuccess={<Settings />}
        pathError="/404"
      />
    ),
  },
];

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    children: routes,
  },
  {
    path: '*',
    element: <NotFound />,
  },
]);

function Routes() {
  return <RouterProvider router={router} />;
}

export default Routes;
