import { Button } from 'flowbite-react';
import { useRef, useState, useContext } from 'react';
import { TbEdit } from 'react-icons/tb';
import DefaultAvatar from '../../assets/default_avatar.jpg';
import { Form, Formik, FormikProps } from 'formik';
import MyInput from '../../components/controls/MyInput';
import { useMutation } from 'react-query';
import { registerUser } from '../../services/api/registrationApi';
import toast from 'react-hot-toast';
import { registrationFormSchema } from '../../schemas/registrationFormSchema';
import { useNavigate, redirect, Navigate } from 'react-router-dom';
import { DataContext } from '../../context/DataContext';

interface FormValues {
  name: string;
  email: string;
  birthDate: string | null;
  avatar: string | null;
}

export default function Registration() {
  const dataContext = useContext(DataContext);

  const [file, setFile] = useState<any>();

  const inputFile = useRef<HTMLInputElement>(null);

  const formRef = useRef<FormikProps<FormValues>>(null);
  const navigate = useNavigate();

  const { mutate: register } = useMutation(
    'register',
    (variables: {
      name: string;
      email: string;
      birthDate: string | null;
      avatar: string | null;
    }) => registerUser(variables),
    {
      onSuccess: (response) => {
        toast.success('Registration completed successfully!');
        dataContext.update = dataContext.update++;
        navigate('/');
      },
      onError: (error: Error) => {
        toast.error(error.message);
      },
    }
  );

  const handleClick = () => {
    inputFile.current?.click();
  };

  function handleChange(event: any) {
    setFile(URL.createObjectURL(event.target.files[0]));
    console.log(file);
  }

  const onSubmit = (values: FormValues, actions: any) => {
    actions.resetForm();
    if (values.birthDate == '') {
      values.birthDate = null;
    }
    register(values);
  };

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current.handleSubmit();
    }
  };

  return (
    <Formik
      innerRef={formRef}
      initialValues={{ name: '', email: '', birthDate: '', avatar: null }}
      validationSchema={() => registrationFormSchema()}
      onSubmit={onSubmit}
    >
      {(formik) => {
        return (
          <Form>
            <div className="flex flex-wrap w-3/4 md:w-2/4 lg:w-1/4 mx-auto">
              <div className=" py-8 w-full">
                <div className="relative flex flex-col min-w-0 break-words bg-white w-full p-10 shadow-lg rounded-2xl border border-gray-100">
                  <div className="flex-auto">
                    <h6 className="text-xl md:text-2xl font-semibold">
                      Please fill in your details
                    </h6>
                    <div
                      className="flex justify-center mx-auto py-5"
                      onClick={handleClick}
                    >
                      <div className="overflow-hidden  aspect-video cursor-pointer rounded-full relative group">
                        <div className="w-full h-full rounded-full opacity-0 group-hover:opacity-40 transition duration-300 ease-in-out cursor-pointer absolute bg-black  text-white">
                          <div className="w-full h-full text-xl group-hover:opacity-100 group-hover:translate-y-0 translate-y-4 transform transition duration-300 ease-in-out">
                            <div className="flex w-full h-full font-bold justify-center items-center">
                              <TbEdit size={60} />
                            </div>
                          </div>
                        </div>

                        <img
                          className="cursor-pointer w-32 h-32 md:w-44 md:h-44 rounded-full object-cover"
                          src={file ? file : DefaultAvatar}
                        />
                        <input
                          type="file"
                          name="avatar"
                          className="hidden"
                          onChange={(event: any) => {
                            handleChange(event);
                            formik.setFieldValue(
                              'avatar',
                              event.currentTarget.files[0]
                            );
                          }}
                          ref={inputFile}
                        />
                      </div>
                    </div>
                    <div className="grid gap-4">
                      <MyInput
                        label="Name"
                        name="name"
                        type="text"
                        placeholder="Name"
                      />
                      <MyInput
                        label="Email"
                        name="email"
                        type="text"
                        placeholder="Email"
                      />
                      <MyInput
                        label="Birth Date"
                        name="birthDate"
                        type="date"
                        placeholder="Birth Date"
                      />
                      <div className="flex justify-end">
                        <Button color="dark" size={'md'} onClick={handleSubmit}>
                          Save
                        </Button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </Form>
        );
      }}
    </Formik>
  );
}
