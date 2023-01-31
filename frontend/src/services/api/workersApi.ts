import { MyAPI } from "../MyAPI";

export const getWorkers = async (
  page: number,
  perPage: number,
  search?: string,
  startDate?: string,
  endDate?: string
) => {
  let request = `User?page=${page}&perPage=${perPage}`;

  if (search && search != "") {
    request += `&search=${search}`;
  }

  //Remover quando API for alterada para nao enviar organizationID
  request += `&idOrg=1`;

  //Managers
  request += `&idRole=3`;

  if (startDate) {
    request += `&startDate=${startDate}`;
  }

  if (endDate) {
    request += `&endDate=${endDate}`;
  }

  const response = await MyAPI().get(request);

  return response.data;
};

export const getWorkerByWallet = async (wallet: string) => {
  if (wallet != "") {
    const response = await MyAPI().get(`User/getUserByWallet?wallet=${wallet}`);

    return response.data;
  }
};

export const saveWorker = async (variables: { email: string; role?: number }) => {
  return await MyAPI().patch(`User/changeUserRole?userEmail=${variables.email}&role=3`);
};

export const removeWorker = async (wallet: string) => {
  if (wallet != "") {
    return await MyAPI().patch(`User/removeUserFromOrganization?wallet=${wallet}`);
  }
};
