import { MyAPI } from "../MyAPI";

export const getOrganizations = async () => {
  let request = "Organization?page=1&perPage=10";
  const response = await MyAPI().get(request);
  return response.data;
};
