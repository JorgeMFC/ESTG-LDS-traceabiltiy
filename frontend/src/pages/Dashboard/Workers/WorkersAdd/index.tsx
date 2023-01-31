import { Button } from "flowbite-react";
import MyCustomDialog from "../../../../components/MyCustomDialog";
import { DialogSizes } from "../../../../models/DialogSizes";
import { Formik, Form, FormikProps } from "formik";
import { useMutation } from "react-query";
import { useRef } from "react";
import { queryClient } from "../../../../services/queryClient";
import { saveWorker } from "../../../../services/api/workersApi";
import { addWorkerFormSchema } from "../../../../schemas/workerFormSchema";
import MyInput from "../../../../components/controls/MyInput";
import toast from "react-hot-toast";

interface WorkersAddProps {
  open: boolean;
  handleVisibility: (visibility: boolean) => void;
}

interface FormValues {
  email: string;
}

export default function WorkersAdd(props: WorkersAddProps) {
  const formRef = useRef<FormikProps<FormValues>>(null);

  const { mutate: workerAdd } = useMutation(
    "addWorker",
    (variables: { email: string }) => saveWorker(variables),
    {
      onSuccess: (response) => {
        queryClient.invalidateQueries({ queryKey: ["getWorkers"] }),
          toast.success("Worker added successfully!");
      },
      onError: (error: Error) => {
        toast.error(error.message);
      },
    }
  );

  const onSubmit = (values: FormValues, actions: any) => {
    actions.resetForm();

    workerAdd(values);
    props.handleVisibility(false);
  };

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current.handleSubmit();
    }
  };

  return (
    <MyCustomDialog
      title="Add a new worker"
      open={props.open}
      handleVisibility={props.handleVisibility}
      content={
        <Formik
          innerRef={formRef}
          initialValues={{ email: "" }}
          validationSchema={() => addWorkerFormSchema()}
          onSubmit={onSubmit}
        >
          <Form>
            <div className="px-5">
              <div className="grid gap-10">
                <div className="grid gap-4">
                  <MyInput label="Email" name="email" type="text" placeholder="Worker email" />
                </div>
              </div>
            </div>
          </Form>
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
