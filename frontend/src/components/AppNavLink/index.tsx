import { useLinkClickHandler, useLocation } from 'react-router-dom';
import { Navbar } from 'flowbite-react';

export interface AppNavLinkProps {
  to: string;
  text: string;
}

export default function AppNavLink(props: AppNavLinkProps) {
  const location = useLocation();
  const clickHandler = useLinkClickHandler(props.to);

  const ValidatePath = () => {
    let atualPath = location.pathname.split('/');
    let verifyPath = props.to.split('/');

    if (atualPath.length < verifyPath.length) {
      return false;
    }

    let count = 0;

    for (let i = 0; i < verifyPath.length; i++) {
      if (atualPath[i] != verifyPath[i]) {
        break;
      }
      count++;
    }

    return count == verifyPath.length;
  };

  return (
    <span onClick={clickHandler}>
      <Navbar.Link href={props.to} active={ValidatePath()}>
        {props.text}
      </Navbar.Link>
    </span>
  );
}
