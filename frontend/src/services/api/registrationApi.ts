import { TypeRequest } from "../../models/api/TypeRequest";
import { MyAPI } from "../MyAPI";

export const registerUser = async (variables: {
  name: string;
  email: string;
  birthDate: string | null;
  avatar: string | null;
}) => {
  return await MyAPI(TypeRequest.FORM).put("Authentication/regist", variables);
};
