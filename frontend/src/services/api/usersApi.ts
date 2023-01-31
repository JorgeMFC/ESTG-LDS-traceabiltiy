import { MyAPI } from '../MyAPI';

export const getUsers = async (
  page: number,
  perPage: number,
  search?: string,
  role?: number,
  organization?: number,
  startDate?: string,
  endDate?: string
) => {
  let request = `User?page=${page}&perPage=${perPage}`;

  if (search && search != '') {
    request += `&search=${search}`;
  }

  if (organization && organization > 0) {
    request += `&idOrg=${organization}`;
  }

  if (role && role > 0) {
    request += `&idRole=${role}`;
  }

  if (startDate) {
    request += `&startDate=${startDate}`;
  }

  if (endDate) {
    request += `&endDate=${endDate}`;
  }

  const response = await MyAPI().get(request);

  return response.data;
};

export const blockUser = async (wallet: string) => {
  return await MyAPI().patch(`User/desactivateUser?wallet=${wallet}`);
};

export const unblockUser = async (wallet: string) => {
  return await MyAPI().patch(`User/activateUser?wallet=${wallet}`);
};

export const getMyUser = async () => {
  const response = await MyAPI().get(`User/getUserInfo`);
  return response.data;
};
