import { DataContext } from './DataContext';

interface props {
  children: JSX.Element;
}

export const DataProvider = ({ children }: props) => {
  return (
    <DataContext.Provider value={{ update: 0 }}>
      {children}
    </DataContext.Provider>
  );
};
