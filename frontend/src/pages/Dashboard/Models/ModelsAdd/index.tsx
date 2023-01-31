import { Button } from "flowbite-react";

import MyCustomDialog from "../../../../components/MyCustomDialog";
import { DialogSizes } from "../../../../models/DialogSizes";
import NoImage from "../../../../assets/noimage.jpg";
import { MyCustomSelect, Option } from "../../../../components/MyCustomSelect";
import { Color, ColorPagination } from "../../../../models/api/Color";
import { Size, SizePagination } from "../../../../models/api/Size";
import { Form, Formik, FormikProps } from "formik";
import { useRef, useState } from "react";
import { StagesModelPaginated } from "../../../../models/api/StagesModel";
import { Component } from "../../../../models/api/Component";
import { addModelFormSchema } from "../../../../schemas/modelFormSchema";
import MyInput from "../../../../components/controls/MyInput";
import MySelect from "../../../../components/controls/MySelect";
import { ComponentsReferences } from "../ModelsList";
import { useMutation } from "react-query";
import { queryClient } from "../../../../services/queryClient";
import toast from "react-hot-toast";
import { Model } from "../../../../models/api/Model";
import { addModel } from "../../../../services/api/modelApi";
import { TbEdit } from "react-icons/tb";

interface ModelsAddProps {
  colors: ColorPagination | undefined;
  sizes: SizePagination | undefined;
  productionProcesses: StagesModelPaginated | undefined;
  componentsReferences: ComponentsReferences | undefined;
  open: boolean;
  handleVisibility: (visibility: boolean) => void;
}

interface FormValues {
  id: string;
  name: string;
  modelPhoto: string | null;
  colors: Array<Option>;
  sizes: Array<Option>;
  productionProcess: string;
  components: Array<string>;
}

export default function ModelsAdd(props: ModelsAddProps) {
  const [file, setFile] = useState<any>();

  const inputFile = useRef<HTMLInputElement>(null);
  const formRef = useRef<FormikProps<FormValues>>(null);

  const modelAdd = useMutation(addModel, {
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["getModels"] });
      toast.success("Model added successfully!");
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const handleClick = () => {
    inputFile.current?.click();
  };

  function handleChange(event: any) {
    setFile(URL.createObjectURL(event.target.files[0]));
  }

  const colorsId: string[] = [];
  props.colors?.results.forEach((color) => {
    colorsId.push(color.id.toString());
  });

  const colorsOptions: Array<Option> = [];
  props.colors?.results.map((color) => {
    colorsOptions.push({ value: color.id, label: color.color1 });
  });
  const [colorsValue, setColorsValue] = useState<Array<Option>>(colorsOptions);

  const sizesId: string[] = [];
  props.sizes?.results.forEach((size) => {
    sizesId.push(size.id.toString());
  });

  const sizesOptions: Array<Option> = [];
  props.sizes?.results.map((size) => {
    sizesOptions.push({ value: size.id, label: size.size1 });
  });
  const [sizesValue, setSizesValue] = useState<Array<Option>>(sizesOptions);

  const productionProcessesId: string[] = [];
  props.productionProcesses?.results.forEach((productionProcess) => {
    productionProcessesId.push(productionProcess.id.toString());
  });

  const onSubmit = (values: FormValues, actions: any) => {
    props.handleVisibility(false);
    actions.resetForm();
    setFile(null);

    const model: Model = {
      id: values.id,
      organizationId: 1,
      name: values.name,
      deletedAt: null,
      stagesModelId: values.productionProcess,
      modelPhoto: "image",
    };

    const colorsAux: Array<Color> = [];

    props.colors?.results.map((propsColor) => {
      values.colors.map((color) => {
        if (propsColor.id == color.value) {
          colorsAux.push(propsColor);
          return;
        }
      });
    });

    const sizesAux: Array<Size> = [];

    props.sizes?.results.map((propsSize) => {
      values.sizes.map((size) => {
        if (propsSize.id == size.value) {
          sizesAux.push(propsSize);
          return;
        }
      });
    });

    const componentsAux: Array<Component> = [];

    props.componentsReferences?.sole.map((elem) => {
      if (elem.id == values.components[0]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.toeCap.map((elem) => {
      if (elem.id == values.components[1]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.tongue.map((elem) => {
      if (elem.id == values.components[2]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.insole.map((elem) => {
      if (elem.id == values.components[3]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.shoelace.map((elem) => {
      if (elem.id == values.components[4]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.eyelets.map((elem) => {
      if (elem.id == values.components[5]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.counterLining.map((elem) => {
      if (elem.id == values.components[6]) {
        componentsAux.push(elem);
        return;
      }
    });

    props.componentsReferences?.heel.map((elem) => {
      if (elem.id == values.components[7]) {
        componentsAux.push(elem);
        return;
      }
    });

    modelAdd.mutate({
      model: model,
      image: values.modelPhoto ? values.modelPhoto : null,
      colors: colorsAux,
      sizes: sizesAux,
      components: componentsAux,
    });
  };

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current.handleSubmit();
    }
  };

  return (
    <MyCustomDialog
      title="Add Model"
      open={props.open}
      handleVisibility={props.handleVisibility}
      content={
        <Formik
          innerRef={formRef}
          initialValues={{
            id: "",
            name: "",
            modelPhoto: null,
            colors: [],
            sizes: [],
            productionProcess: "",
            components: ["", "", "", "", "", "", "", ""],
          }}
          validationSchema={() => addModelFormSchema(colorsId, sizesId, productionProcessesId)}
          onSubmit={onSubmit}
        >
          {(formik) => (
            <Form>
              <div className="px-5">
                <div className="flex justify-center sm:hidden">
                  <img
                    src={file ? file : NoImage}
                    className="block object-cover shadow rounded align-middle border-none h-4/6 w-4/6 max-h-52"
                  />
                </div>
                <div className="grid gap-10">
                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <MyInput label="Id" name="id" type="text" placeholder="Model id" />
                    <div
                      className="flex justify-center sm:row-span-5 mx-auto py-5"
                      onClick={handleClick}
                    >
                      <div className="overflow-hidden sm:row-span-5 aspect-video cursor-pointer relative group">
                        <div className="w-full h-full sm:row-span-5  opacity-0 group-hover:opacity-40 transition duration-300 ease-in-out cursor-pointer absolute bg-black  text-white">
                          <div className="w-full h-full sm:row-span-5  text-xl group-hover:opacity-100 group-hover:translate-y-0 translate-y-4 transform transition duration-300 ease-in-out">
                            <div className="flex w-full sm:row-span-5  h-full font-bold justify-center items-center">
                              <TbEdit size={60} />
                            </div>
                          </div>
                        </div>

                        <img
                          className="hidden sm:block sm:row-span-5 object-cover shadow rounded align-middle border-none h-full w-full max-h-54"
                          src={file ? file : NoImage}
                        />
                        <input
                          type="file"
                          name="modelPhoto"
                          className="hidden"
                          onChange={(event: any) => {
                            handleChange(event);
                            formik.setFieldValue("modelPhoto", event.currentTarget.files[0]);
                          }}
                          ref={inputFile}
                        />
                      </div>
                    </div>

                    <MyInput label="Name" name="name" type="text" placeholder="Model name" />

                    <div>
                      <label className="block text-sm font-medium text-gray-700">Colors</label>
                      <MyCustomSelect name="colors" isMulti={true} options={colorsValue} />
                    </div>

                    <div>
                      <label className="block text-sm font-medium text-gray-700">Sizes</label>
                      <MyCustomSelect name="sizes" isMulti={true} options={sizesValue} />
                    </div>

                    <MySelect label="Production Process" name={"productionProcess"}>
                      <option disabled value="">
                        Select a production process
                      </option>
                      {props.productionProcesses?.results.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Sole" name={"components.0"}>
                      <option disabled value="">
                        Select a sole reference
                      </option>
                      {props.componentsReferences?.sole.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Toe Cap" name={"components.1"}>
                      <option disabled value="">
                        Select a toe cap reference
                      </option>
                      {props.componentsReferences?.toeCap.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Tongue" name={"components.2"}>
                      <option disabled value="">
                        Select a tongue reference
                      </option>
                      {props.componentsReferences?.tongue.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Insole" name={"components.3"}>
                      <option disabled value="">
                        Select a insole reference
                      </option>
                      {props.componentsReferences?.insole.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Shoelace" name={"components.4"}>
                      <option disabled value="">
                        Select a shoelace reference
                      </option>
                      {props.componentsReferences?.shoelace.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Eyelets" name={"components.5"}>
                      <option disabled value="">
                        Select a eyelets reference
                      </option>
                      {props.componentsReferences?.eyelets.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Counter lining" name={"components.6"}>
                      <option disabled value="">
                        Select a counter lining reference
                      </option>
                      {props.componentsReferences?.counterLining.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>

                    <MySelect label="Heel" name={"components.7"}>
                      <option disabled value="">
                        Select a heel reference
                      </option>
                      {props.componentsReferences?.heel.map((option) => (
                        <option key={option.id} value={option.id}>
                          {option.id}
                        </option>
                      ))}
                    </MySelect>
                  </div>
                </div>
              </div>
            </Form>
          )}
        </Formik>
      }
      actions={
        <div>
          <div className="flex flex-row-reverse gap-2">
            <Button color="dark" size="md" onClick={handleSubmit}>
              Save
            </Button>
            <Button
              color="dark"
              size="md"
              onClick={() => {
                setFile(null), props.handleVisibility(false);
              }}
            >
              Close
            </Button>
          </div>
        </div>
      }
      size={DialogSizes.MEDIUM}
    />
  );
}
