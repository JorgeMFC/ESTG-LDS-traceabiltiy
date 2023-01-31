import { Component } from "../../models/api/Component";
import { MyAPI } from "../MyAPI";

export const getComponents = async () => {
  const response = await MyAPI().get("Component/1");
  return response.data;
};

export const getComponentsWithoutProvider = async () => {
  const response = await MyAPI().get("Component/getComponentsWithoutProvider");
  return response.data;
};

export const getOwnComponents = async (providerId: string) => {
  if (providerId != "") {
    const response = await MyAPI().get(
      `Component/getComponentsByproviderId?providerId=${providerId}`
    );
    return response.data;
  }
};

export const getComponentsByType = async (props: { componentTypeId: number }) => {
  const response = await MyAPI().get(
    `Component/getComponentsByTypeId?componentTypeId=${props.componentTypeId}`
  );
  return response.data;
};

export const getComponentsByModel = async (id: string) => {
  if (id != "") {
    const response = await MyAPI().get(`Component/getComponentsByModelId?modelId=${id}`);
    return response.data;
  }
};

export const addComponent = async (props: Component) => {
  return await MyAPI().post("/Component", props);
};

export const updateComponent = async (props: Component) => {
  return await MyAPI().put(`/Component`, props);
};

export const deleteComponent = async (props: { id: String }) => {
  return await MyAPI().patch(`/Component/disable?id=${props.id}`);
};
