import * as yup from "yup";

export const addWorkerFormSchema = () => {
  return yup.object().shape({
    email: yup.string().email().required("Required"),
  });
};
