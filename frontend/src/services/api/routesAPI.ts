import { MyAPI } from '../MyAPI';

const base = 'Routes/';

export const isPermittedRoute = async (props: { route: string }) => {
  const response = await MyAPI().post(
    `${base}IsPermittedRoute`,
    `"${props.route}"`
  );
  return response.data;
};

export const isPermittedRoutes = async (props: { routes: Array<string> }) => {
  const response = await MyAPI().post(`${base}IsPermittedRoutes`, props.routes);
  return response.data;
};
