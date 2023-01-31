import { MyAPI } from "../MyAPI";

const base = "Authentication/";

export const getSignatureMessageRequest = async (props: { wallet: string }) => {
  const response = await MyAPI().post(`${base}getSignatureMessage`, `"${props.wallet}"`);
  return response.data;
};

export const loginRequest = async (props: { wallet: string; signature: string }) => {
  const response = await MyAPI().post(`${base}login`, props);
  return response.data;
};
