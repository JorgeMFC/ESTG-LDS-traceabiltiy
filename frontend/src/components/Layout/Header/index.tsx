import { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import { DataContext } from '../../../context/DataContext';

import toast from 'react-hot-toast';

import { loginRequest } from '../../../services/api/authenticationApi';

import { metaSignMessage } from '../../../services/Metamask';

import { Dropdown, Navbar } from 'flowbite-react';
import { Button } from 'flowbite-react';
import AppNavLink from '../../AppNavLink';

import icon from '../../../assets/icon.svg';
import metamask from '../../../assets/MetaMask.png';
import default_avatar from '../../../assets/default_avatar.jpg';
import { Link } from 'react-router-dom';
import { isPermittedRoutes } from '../../../services/api/routesAPI';
import { User } from '../../../models/api/UserModel';
import { getMyUser } from '../../../services/api/usersApi';

export default function Header() {
  const [isLogged, setIsLogged] = useState<Boolean>(
    sessionStorage.getItem('token') != null
  );
  const dataContext = useContext(DataContext);
  const navigate = useNavigate();

  const routesVerify = [
    '/',
    '/dashboard',
    '/dashboard/users',
    '/orders',
    '/settings',
  ];

  const [routes, setRoutes] = useState<string[]>([]);
  const [user, setUser] = useState<User | null>(null);

  const login = async () => {
    try {
      var data = await metaSignMessage();
      const token = await loginRequest(data);
      sessionStorage.setItem('token', token);
      setIsLogged(true);
      navigate('/');
    } catch (err) {
      toast.error((err as Error).message);
    }
  };

  const logout = () => {
    sessionStorage.removeItem('token');
    setIsLogged(false);
    dataContext.update++;
    navigate('/');
  };

  useEffect(() => {
    isPermittedRoutes({
      routes: routesVerify,
    })
      .then((result) => {
        let count = 0;
        setRoutes([]);
        for (const resultRoute of result) {
          count++;
          setRoutes((arry) => [...arry, resultRoute.route]);
        }

        if (isLogged && count == 0) {
          navigate('/registration');
        }
      })
      .catch(() => {});

    if (isLogged) {
      getMyUser()
        .then((result) => {
          setUser(result);
        })
        .catch(() => {
          setIsLogged(false);
        });
    }
  }, [isLogged, dataContext.update]);

  return (
    <Navbar fluid={true} rounded={true} className="bg-white shadow my-auto">
      <Navbar.Brand className="lg:pl-10" href="/">
        <img src={icon} className="mr-3 h-6 sm:h-9" />
        <span className="self-center whitespace-nowrap text-xl font-bold text-current">
          MyTrace
        </span>
      </Navbar.Brand>
      <div className="flex md:order-2 lg:pr-10">
        {!isLogged && (
          <Button
            size="xs"
            color="dark"
            className="w-32 !bg-[#111B47] !hover:bg-[#0c1330]"
            onClick={login}
          >
            <img src={metamask} className="h-7 pr-3" />
            <span className="my-auto">Login</span>
          </Button>
        )}
        {isLogged && (
          <Dropdown
            arrowIcon={false}
            inline={true}
            label={
              <img
                className="rounded-full h-12 w-12 object-cover"
                src={
                  user && user.avatar
                    ? `${import.meta.env.BASE_URL}${user.avatar}`
                    : default_avatar
                }
                onError={({ currentTarget }) => {
                  currentTarget.onerror = null; // prevents looping
                  currentTarget.src = default_avatar;
                }}
              />
            }
          >
            {user && (
              <Dropdown.Header>
                <span className="block text-sm">{user?.name}</span>
                <span className="block truncate text-sm font-medium">
                  {user?.email}
                </span>
              </Dropdown.Header>
            )}

            {routes.indexOf('/') !== -1 && (
              <Link to="/settings">
                <Dropdown.Item>Settings</Dropdown.Item>
              </Link>
            )}

            <Dropdown.Divider />
            <Dropdown.Item onClick={logout}>Sign out</Dropdown.Item>
          </Dropdown>
        )}
        <Navbar.Toggle />
      </div>
      <Navbar.Collapse>
        {routes.indexOf('/') !== -1 && <AppNavLink to="/" text="Home" />}
        {routes.indexOf('/dashboard') !== -1 && (
          <AppNavLink to="/dashboard" text="Dashboard" />
        )}
        {routes.indexOf('/dashboard/users') !== -1 && (
          <AppNavLink to="/dashboard/users" text="Dashboard" />
        )}
        {routes.indexOf('/orders') !== -1 && (
          <AppNavLink to="/orders" text="Orders" />
        )}
      </Navbar.Collapse>
    </Navbar>
  );
}
