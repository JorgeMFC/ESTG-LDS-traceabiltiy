import { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { Puff } from 'react-loader-spinner';
import { isPermittedRoute } from './services/api/routesAPI';

export interface AppNavLinkProps {
  path: string;
  childrenSuccess: JSX.Element;
  pathError: string;
}

export default function PrivateRoute(props: AppNavLinkProps) {
  const [isLoading, setIsLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    isPermittedRoute({
      route: props.path,
    })
      .then((result) => {
        setIsLoading(false);
        if (result.permissions != 'UnAuthorized') {
          setIsAuthenticated(true);
        }
      })
      .catch(() => {
        setIsLoading(false);
      });
  });

  if (isLoading) {
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
  } else if (isAuthenticated) {
    return props.childrenSuccess;
  } else {
    return <Navigate to={props.pathError} replace={true} />;
  }
}
