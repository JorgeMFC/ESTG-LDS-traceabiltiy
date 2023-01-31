import axios from 'axios';
import { TypeRequest } from '../models/api/TypeRequest';

function configuration(typeRequest: TypeRequest = TypeRequest.JSON): {} {
  const token = sessionStorage.getItem('token');

  let config: any = {};

  if (typeRequest == TypeRequest.FORM) {
    config['Content-type'] = 'multipart/form-data';
  } else {
    config['Content-type'] = 'application/json';
  }

  if (token) {
    config.Authorization = `Bearer ${token}`;
  }
  return config;
}

export function MyAPI(typeRequest: TypeRequest = TypeRequest.JSON) {
  const baseURL = `${import.meta.env.VITE_BASE_URL_API}api/`;

  return axios.create({
    baseURL: baseURL,
    timeout: 5000,
    headers: configuration(typeRequest),
  });
}
