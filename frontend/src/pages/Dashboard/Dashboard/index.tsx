import { NavLink } from 'react-router-dom';
import { Outlet } from 'react-router-dom';
import { useState, useEffect } from 'react';

import './style.scss';

import { Puff } from 'react-loader-spinner';

import {
  MdNearMe as TrackingIcon,
  MdGroup as WorkersIcon,
  MdManageAccounts as UsersIcon,
} from 'react-icons/md';
import { IoExtensionPuzzle as ComponentsIcon } from 'react-icons/io5';
import { AiOutlineCodeSandbox as ModelsIcon } from 'react-icons/ai';
import { HiBuildingOffice2 as OrganizationsIcon } from 'react-icons/hi2';
import {
  TbTimeline as ProductionProcessesIcon,
  TbTruckDelivery as ProvidersIcon,
} from 'react-icons/tb';
import { FaBuilding as OrganizationIcon } from 'react-icons/fa';
import { FaUsers as ClientsIcon } from 'react-icons/fa';
import { isPermittedRoutes } from '../../../services/api/routesAPI';

import { RouteHash } from '../../../models/RouteHash';
import { Route } from '../../../models/Route';

function getRoutes(iconsSize: number): RouteHash {
  let routes: RouteHash = {};
  const baseURL = '/dashboard';

  routes[baseURL] = {
    to: '',
    icon: <TrackingIcon size={iconsSize} />,
    text: 'Tracking',
  };
  routes[`${baseURL}/models`] = {
    to: 'models',
    icon: <ModelsIcon size={iconsSize} />,
    text: 'Models',
  };
  routes[`${baseURL}/providers`] = {
    to: 'providers',
    icon: <ProvidersIcon size={iconsSize} />,
    text: 'Providers',
  };
  routes[`${baseURL}/components`] = {
    to: 'components',
    icon: <ComponentsIcon size={iconsSize} />,
    text: 'Components',
  };
  routes[`${baseURL}/production_processes`] = {
    to: 'production_processes',
    icon: <ProductionProcessesIcon size={iconsSize} />,
    text: 'Production Processes',
  };
  routes[`${baseURL}/workers`] = {
    to: 'workers',
    icon: <WorkersIcon size={iconsSize} />,
    text: 'Workers',
  };
  routes[`${baseURL}/users`] = {
    to: 'users',
    icon: <UsersIcon size={iconsSize} />,
    text: 'Users',
  };
  routes[`${baseURL}/organizations`] = {
    to: 'organizations',
    icon: <OrganizationsIcon size={iconsSize} />,
    text: 'Organizations',
  };
  routes[`${baseURL}/organization`] = {
    to: 'organization',
    icon: <OrganizationIcon size={iconsSize} />,
    text: 'Organization',
  };
  routes[`${baseURL}/clients`] = {
    to: 'clients',
    icon: <ClientsIcon size={iconsSize} />,
    text: 'Clients',
  };

  return routes;
}

export default function Dashboard() {
  const [isLoading, setIsLoading] = useState(true);
  const iconsSize = 25;

  const routes: RouteHash = getRoutes(iconsSize);

  const [routesUse, setRoutesUse] = useState<Route[]>([]);

  useEffect(() => {
    let routesVerify: string[] = [];
    for (const route in routes) {
      routesVerify.push(route);
    }

    isPermittedRoutes({
      routes: routesVerify,
    })
      .then((result) => {
        setRoutesUse([]);
        for (const resultRoute of result) {
          setRoutesUse((arry) => [...arry, routes[resultRoute.route]]);
        }
        setIsLoading(false);
      })
      .catch(() => {
        setIsLoading(false);
      });
  }, []);

  if (!isLoading) {
    return (
      <>
        <div className="hidden md:block pt-2 pb-2 h-full">
          <div className="grid grid-cols-12 gap-2 h-full">
            <div className="lg:col-span-2 col-span-3 h-full">
              <div className="h-full max-h-screen sticky top-0 align-top shadow rounded-r-3xl space-y-4 p-5 bg-white">
                {routesUse.map((item, index) => (
                  <NavLink
                    key={index}
                    to={item.to}
                    end
                    className={({ isActive }) =>
                      isActive
                        ? 'dashboard-menu-selected'
                        : 'dashboard-menu-not-selected'
                    }
                  >
                    {item.icon}
                    {item.text}
                  </NavLink>
                ))}
              </div>
            </div>
            <div className="lg:col-span-10 col-span-9 px-5">
              <Outlet />
            </div>
          </div>
        </div>
        <div className="md:hidden pt-2 pb-2">
          <div className="flex gap-2 p-2 h-full overflow-auto border-b border-gray-200">
            {routesUse.map((item, index) => (
              <NavLink
                key={index}
                to={item.to}
                end
                className={({ isActive }) =>
                  isActive
                    ? 'dashboard-menu-selected'
                    : 'dashboard-menu-not-selected'
                }
              >
                {item.icon}
                {item.text}
              </NavLink>
            ))}
          </div>
          <div className="p-5">
            <Outlet />
          </div>
        </div>
      </>
    );
  } else {
    return (
      <div className="w-full h-full flex justify-center items-center">
        <Puff
          height="150"
          width="150"
          radius={1}
          color="#3b82f6"
          visible={true}
        />
      </div>
    );
  }
}
