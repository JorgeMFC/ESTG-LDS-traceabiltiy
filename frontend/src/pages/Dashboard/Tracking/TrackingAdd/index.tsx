import { Button } from 'flowbite-react';

import { Form, Formik, FormikProps } from 'formik';
import { useRef, useState } from 'react';
import toast from 'react-hot-toast';
import { useMutation, useQuery } from 'react-query';
import NoImage from '../../../../assets/noimage.jpg';
import MyCustomDialog from '../../../../components/MyCustomDialog';
import MyInput from '../../../../components/controls/MyInput';
import MySelect from '../../../../components/controls/MySelect';
import { Address, ClientPaginated } from '../../../../models/api/Client';
import { ModelPagination } from '../../../../models/api/Model';
import { StagesModelPaginated } from '../../../../models/api/StagesModel';
import { publicNFT } from '../../../../services/Blockchain';
import {
  getClientAddressesByClientId,
  getClients,
} from '../../../../services/api/clientApi';
import { addLot, createNFT } from '../../../../services/api/lotsApi';
import { getModels } from '../../../../services/api/modelsApi';
import { getStagesModel } from '../../../../services/api/stagesModelApi';
import { queryClient } from '../../../../services/queryClient';
import { addTrackingFormSchema } from '../../../../schemas/addTrackingFormSchema';

interface TrackingAddProps {
  open: boolean;
  handleVisibility: (visibility: boolean) => void;
}

interface FormValues {
  id: string;
  modelId: string;
  modelColorId: string;
  modelSizeId: string;
  clientId: string;
  clientAddressId: string;
  stagesModelId: string;
  amount: number;
}

//model
//color
//size
//client
//production process

export default function TrackingAdd(props: TrackingAddProps) {
  const [selectedModelId, setSelectedModelId] = useState<string>('');
  const [clientId, setClientId] = useState<string>('');
  const formRef = useRef<FormikProps<FormValues>>(null);

  const {
    isLoading: isLoadingStagesModel,
    isError: isErrorStagesModel,
    error: errorStagesModel,
    data: stagesModel,
  } = useQuery<StagesModelPaginated, Error>('tracking-page-stagesModel', () =>
    getStagesModel(1, 1000)
  );

  const stagesModelIds: Array<string> = [];
  stagesModel?.results.forEach((stageModel) =>
    stagesModelIds.push(stageModel.id)
  );

  const {
    isLoading: isLoadingClients,
    isError: isErrorClients,
    error: errorClients,
    data: clients,
  } = useQuery<ClientPaginated, Error>('tracking-page-clients', () =>
    getClients(1, 1000)
  );

  const clientsIds: Array<string> = [];
  clients?.results.forEach((client) => clientsIds.push(client.id));

  const {
    isLoading: isLoadingModels,
    isError: isErrorModels,
    error: errorModels,
    data: models,
  } = useQuery<ModelPagination, Error>('tracking-page-models', () =>
    getModels(1, 1000)
  );

  const modelsIds: Array<string> = [];
  models?.results.forEach((model) => modelsIds.push(model.model.id));

  const {
    isLoading: isLoadingClientAddresses,
    isError: isErrorClientAddresses,
    error: errorClientAddresses,
    data: clientAddresses,
  } = useQuery<Array<Address>, Error>(
    ['tracking-page-client-addresses', clientId],
    () => getClientAddressesByClientId(clientId || '0')
  );

  const clientAddressesIds: Array<string> = [];
  clientAddresses?.forEach((clientAddress) =>
    clientAddressesIds.push(clientAddress.id)
  );

  const selectedModel =
    selectedModelId != ''
      ? models?.results.filter((model) => model.model.id === selectedModelId)[0]
      : { colors: [], sizes: [] };

  const colorsIds: Array<string> = [];
  selectedModel?.colors?.forEach((color) => colorsIds.push(color.id));

  const sizesIds: Array<string> = [];
  selectedModel?.sizes?.forEach((size) => sizesIds.push(size.id));

  const addLotMutation = useMutation(addLot, {
    onSuccess: (response) => {
      // Invalidates cache and refetch
      queryClient.invalidateQueries('tracking-page-lots');
      toast.success('Lot inserted successfully!');
    },
    onError: (error: Error) => {
      toast.error(error.message);
    },
  });

  const onSubmit = async (values: FormValues, actions: any) => {
    props.handleVisibility(false);
    actions.resetForm();

    let lot = {
      id: values.id,
      organizationId: 1,
      modelId: values.modelId,
      modelColorId: values.modelColorId,
      modelSizeId: values.modelSizeId,
      clientId: values.clientId,
      clientAddressId: values.clientAddressId,
      organizationAddressId: 'OADD0001',
      deliveryDate: null,
      lotSize: values.amount,
      hash: '',
      stagesModelId: values.stagesModelId,
      canceledAt: null,
    };

    console.log(lot);

    const responseNFT = await createNFT(lot);

    let realHash = '';
    const responsePublishNft = await publicNFT(
      '0xe955c5100f244957b41d11167932bc1caadb9eb1',
      responseNFT,
      (hash) => {
        realHash = hash;
      },
      (error) => {
        console.log(error);
      }
    );

    lot.hash = realHash;

    console.log(lot);

    addLotMutation.mutate(lot);
  };

  const handleSubmit = () => {
    if (formRef.current) {
      formRef.current.handleSubmit();
    }
  };

  return (
    <MyCustomDialog
      title="Add New Order"
      open={props.open}
      handleVisibility={props.handleVisibility}
      content={
        <Formik
          innerRef={formRef}
          initialValues={{
            id: '',
            modelId: '',
            modelColorId: '',
            modelSizeId: '',
            clientId: '',
            clientAddressId: '',
            stagesModelId: '',
            amount: 0,
          }}
          validationSchema={() =>
            addTrackingFormSchema(
              modelsIds,
              colorsIds,
              sizesIds,
              clientsIds,
              clientAddressesIds,
              stagesModelIds
            )
          }
          onSubmit={onSubmit}
          enableReinitialize
        >
          <Form>
            <div className="px-5">
              <div className="flex flex-wrap justify-center mb-8">
                <img
                  src={NoImage}
                  className="w-80 h-48 rounded-lg object-cover"
                />
              </div>

              <div className="grid md:grid-cols-2 gap-x-4 gap-y-4">
                <div>
                  <MyInput
                    label="Reference"
                    name="id"
                    type="text"
                    placeholder="Reference"
                  />
                </div>
                <div>
                  <MyInput
                    label="Amount"
                    name="amount"
                    type="number"
                    placeholder="Amount"
                  />
                </div>

                <div>
                  <MySelect
                    label="Model"
                    name="modelId"
                    getFieldValue={setSelectedModelId}
                  >
                    <option disabled value="">
                      Please select a model
                    </option>
                    {models?.results.map((option) => (
                      <option key={option.model.id} value={option.model.id}>
                        {option.model.name}
                      </option>
                    ))}
                  </MySelect>
                </div>

                <div>
                  <MySelect label="Color" name="modelColorId">
                    <option disabled value="">
                      Please select a color
                    </option>
                    {selectedModel?.colors?.map((option: any) => (
                      <option key={option.id} value={option.id}>
                        {option.color1}
                      </option>
                    ))}
                  </MySelect>
                </div>

                <div>
                  <MySelect label="Size" name="modelSizeId">
                    <option disabled value="">
                      Please select a size
                    </option>
                    {selectedModel?.sizes?.map((option: any) => (
                      <option key={option.id} value={option.id}>
                        {option.size1}
                      </option>
                    ))}
                  </MySelect>
                </div>

                <div>
                  <MySelect
                    label="Client"
                    name="clientId"
                    getFieldValue={setClientId}
                  >
                    <option disabled value="">
                      Please select a client
                    </option>
                    {clients?.results.map((option) => (
                      <option key={option.id} value={option.id}>
                        {option.email}
                      </option>
                    ))}
                  </MySelect>
                </div>

                <div>
                  <MySelect label="Production Process" name="stagesModelId">
                    <option disabled value="">
                      Please select a production process
                    </option>
                    {stagesModel?.results.map((option) => (
                      <option key={option.id} value={option.id}>
                        {option.stagesModelName}
                      </option>
                    ))}
                  </MySelect>
                </div>

                <div>
                  <MySelect label="Address" name="clientAddressId">
                    <option disabled value="">
                      Please select an address
                    </option>
                    {clientAddresses?.map((option) => (
                      <option key={option.id} value={option.id}>
                        {option.address}
                      </option>
                    ))}
                  </MySelect>
                </div>

                <div className="opacity-0 pointer-events-none">
                  <label className="block text-sm font-medium text-gray-700">
                    Empty
                  </label>
                  <div className="relative mt-1 rounded-md shadow-sm">
                    <input
                      type="text"
                      className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                      placeholder="Empty"
                    />
                  </div>
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
            <Button
              color="dark"
              size="md"
              onClick={() => props.handleVisibility(false)}
            >
              Cancel
            </Button>
          </div>
        </div>
      }
    />
  );
}
