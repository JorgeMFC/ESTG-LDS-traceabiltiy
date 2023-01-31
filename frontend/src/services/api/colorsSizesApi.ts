import { MyAPI } from "../MyAPI";

export const getColors = async () => {
  const response = await MyAPI().get(`ColorsSizes/getColors?page=1&perPage=100`);
  return response.data;
};

export const getColorsByModel = async (id: string) => {
  if (id != "") {
    const response = await MyAPI().get(`ColorsSizes/getColorsByModelId?modelId=${id}`);
    return response.data;
  }
};

export const getSizes = async () => {
  const response = await MyAPI().get(`ColorsSizes/getSizes?page=1&perPage=100`);
  return response.data;
};

export const getSizesByModel = async (id: string) => {
  if (id != "") {
    const response = await MyAPI().get(`ColorsSizes/getSizesByModelId?modelId=${id}`);
    return response.data;
  }
};
