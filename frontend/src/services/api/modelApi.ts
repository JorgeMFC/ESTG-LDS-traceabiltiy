import { Color } from "../../models/api/Color";
import { Component } from "../../models/api/Component";
import { Model } from "../../models/api/Model";
import { Size } from "../../models/api/Size";
import { TypeRequest } from "../../models/api/TypeRequest";
import { ComponentsReferences } from "../../pages/Dashboard/Models/ModelsList";
import { MyAPI } from "../MyAPI";

interface Props {
  model: Model;
  image: string | null;
  colors: Array<Color>;
  sizes: Array<Size>;
  components: Array<Component>;
}

export const getModels = async (page: number, perPage: number, search?: string) => {
  let request = `Model?page=${page}&perPage=${perPage}`;

  if (search && search != "") {
    request += `&search=${search}`;
  }

  const response = await MyAPI().get(request);

  return response.data;
};

export const addModel = async (props: Props) => {
  return await MyAPI(TypeRequest.FORM).post("Model", props);
};

export const editModel = async (props: Props) => {
  return await MyAPI(TypeRequest.FORM).put("Model", props);
};

export const modelOutOfProduction = async (id: string) => {
  if (id != "") {
    return await MyAPI().put(`Model/outOfProduction?id=${id}`);
  }
};

export const getComponentsReferences = async () => {
  let result: ComponentsReferences = {
    sole: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=0`)).data,
    toeCap: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=1`)).data,
    tongue: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=2`)).data,
    insole: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=3`)).data,
    shoelace: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=4`)).data,
    eyelets: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=5`)).data,
    counterLining: await (
      await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=6`)
    ).data,
    heel: await (await MyAPI().get(`Component/getComponentsByTypeId?componentTypeId=7`)).data,
  };
  return result;
};
