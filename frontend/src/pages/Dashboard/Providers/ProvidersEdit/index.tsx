import { Button } from "flowbite-react";
import { useRef } from "react";
import MyCustomDialog from "../../../../components/MyCustomDialog";
import { DialogSizes } from "../../../../models/DialogSizes";
import { Component } from "../../../../models/api/Component";
import { FieldArray, Form, Formik, FormikProps } from "formik";
import { useMutation } from "react-query";
import { queryClient } from "../../../../services/queryClient";
import { updateProvider } from "../../../../services/api/providersApi";
import toast from "react-hot-toast";
import { Provider } from "../../../../models/api/Provider";
import { addProviderFormSchema } from "../../../../schemas/providerFormSchema";
import MyInput from "../../../../components/controls/MyInput";
import MySelect from "../../../../components/controls/MySelect";
import { MdDeleteForever } from "react-icons/md";

interface ProvidersEditProps {
  provider: Provider;
  ownComponents: Array<Component>;
  components: Array<Component>;
  open: boolean;
  handleVisibility: (visibility: boolean) => void;
}

interface ComponentsFormValues {
  componentId: string;
}

interface FormValues {
  providerId: string;
  providerName: string;
  providerEmail: string;
  components: Array<ComponentsFormValues>;
}

export default function ProvidersEdit(props: ProvidersEditProps) {
  const formRef = useRef<FormikProps<FormValues>>(null);

  const providerUpdate = useMutation(updateProvider, {
    onSuccess: (response) => {
      queryClient.invalidateQueries({ queryKey: ["getProviders"] });
      queryClient.invalidateQueries({ queryKey: ["getOwnComponents"] });
      queryClient.invalidateQueries({ queryKey: ["getComponentsWithoutProvider"] });
      toast.success("Provider updated successfully!");
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const componentsId: string[] = [];
  const initialComponents: Array<ComponentsFormValues> = [];
  props.ownComponents.forEach((elem) => {
    componentsId.push(elem.id.toString());
    initialComponents.push({ componentId: elem.id.toString() });
  });
  props.components.forEach((elem) => {
    componentsId.push(elem.id.toString());
  });

  const onSubmit = (values: FormValues, actions: any) => {
    props.handleVisibility(false);
    actions.resetForm();

    const componentsAux: Array<Component> = [];

    props.components.map((propsComponent: Component) => {
      values.components.map((component) => {
        if (propsComponent.id == component.componentId) {
          componentsAux.push(propsComponent);
          return;
        }
      });
    });

    props.ownComponents.map((ownComponent) => {
      values.components.map((component) => {
        if (ownComponent.id == component.componentId) {
          componentsAux.push(ownComponent);
          return;
        }
      });
    });

    providerUpdate.mutate({
      provider: {
        id: values.providerId,
        organizationId: props.provider.organizationId,
        name: values.providerName,
        email: values.providerEmail,
        createdAt: props.provider.createdAt,
        deletedAt: props.provider.deletedAt,
      },
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
      title="Edit provider"
      open={props.open}
      handleVisibility={props.handleVisibility}
      content={
        <Formik
          innerRef={formRef}
          initialValues={{
            providerId: props.provider.id,
            providerName: props.provider.name,
            providerEmail: props.provider.email,
            components: initialComponents,
          }}
          validationSchema={() => addProviderFormSchema(componentsId)}
          onSubmit={onSubmit}
          enableReinitialize
        >
          {({ values }) => (
            <Form>
              <div className="px-5">
                <div className="grid gap-10">
                  <div className="grid gap-4">
                    <MyInput
                      label="Id"
                      name="providerId"
                      type="text"
                      placeholder="Provider id"
                      disabled
                    />
                    <MyInput
                      label="Name"
                      name="providerName"
                      type="text"
                      placeholder="Provider name"
                    />
                    <MyInput
                      label="Email"
                      name="providerEmail"
                      type="text"
                      placeholder="Provider email"
                    />

                    <label className="block text-sm font-medium text-gray-700">
                      Components Provided
                    </label>
                    <FieldArray name="components">
                      {({ insert, remove, push }) => (
                        <div>
                          {values.components.length > 0 &&
                            values.components.map((component, index) => (
                              <div className="flex items-center mb-3" key={index}>
                                <div className="w-full">
                                  <MySelect name={`components.${index}.componentId`}>
                                    <option disabled value="">
                                      Please select a component
                                    </option>
                                    {props.ownComponents.map((option) => (
                                      <option key={option.id} value={option.id}>
                                        {option.id}
                                      </option>
                                    ))}
                                    {props.components.map((option) => (
                                      <option key={option.id} value={option.id}>
                                        {option.id}
                                      </option>
                                    ))}
                                  </MySelect>
                                </div>
                                <div>
                                  <button
                                    type="button"
                                    className="flex justify-center items-center my-auto"
                                    onClick={() => values.components.length > 1 && remove(index)}
                                  >
                                    <MdDeleteForever fontSize={20} />
                                  </button>
                                </div>
                              </div>
                            ))}

                          <p
                            className="cursor-pointer text-xs font-semibold text-gray-500 mt-6"
                            onClick={() => push({ componentId: "" })}
                          >
                            Add more components
                          </p>
                        </div>
                      )}
                    </FieldArray>
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
            <Button color="dark" size="md" onClick={() => props.handleVisibility(false)}>
              Cancel
            </Button>
          </div>
        </div>
      }
      size={DialogSizes.SMALL}
    />
  );
}
