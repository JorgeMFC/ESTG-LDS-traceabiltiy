import { Component } from "../../models/api/Component";
import { Provider } from "../../models/api/Provider";
import { MyAPI } from "../MyAPI";

interface Props {
  provider: Provider;
  components: Array<Component>;
}

export const getProviders = async (
  page: number,
  perPage: number,
  search?: string,
  startDate?: string,
  endDate?: string
) => {
  let request = `Provider?page=${page}&perPage=${perPage}`;

  if (search && search != "") {
    request += `&search=${search}`;
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

export const addProvider = async (props: Props) => {
  return await MyAPI().post("Provider", props);
};

export const updateProvider = async (props: Props) => {
  return await MyAPI().put("Provider", props);
};

export const removeProvider = async (id: string) => {
  return await MyAPI().patch(`Provider/disable?id=${id}`);
};
