import { MyAPI } from "../MyAPI";

export const getUserTypes = async () => {
  const response = await MyAPI().get("User/getUsersTypes");

  return response.data;
};
