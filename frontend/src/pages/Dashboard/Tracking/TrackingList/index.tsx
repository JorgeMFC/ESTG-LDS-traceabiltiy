import { Button } from 'flowbite-react';
import { useState } from 'react';

import { TrackingIcons } from '../../../../models/TrackingIcons';

import TrackingAdd from '../TrackingAdd';
import TrackingEdit from '../TrackingEdit';

import { AiOutlinePlus } from 'react-icons/ai';
import { IoOptionsOutline } from 'react-icons/io5';
import { Puff } from 'react-loader-spinner';
import { useQuery } from 'react-query';
import CardModel, { CardModelProps } from '../../../../components/CardModel';
import { LotPaginated } from '../../../../models/api/Lot';
import { getLots } from '../../../../services/api/lotsApi';

export default function TrackingList() {
  const [search, setSearch] = useState('');

  const [openAdd, setOpenAdd] = useState(false);
  const handleVisibilityAdd = (visibility: boolean) => {
    setOpenAdd(visibility);
  };

  const [openEdit, setOpenEdit] = useState(false);
  const handleVisibilityEdit = (visibility: boolean) => {
    setOpenEdit(visibility);
  };

  const [isFiltersVisible, setIsFiltersVisible] = useState(false);

  const {
    isLoading: isLoadingLots,
    isError: isErrorLots,
    error: errorLots,
    data: lotsResult,
  } = useQuery<LotPaginated, Error>(
    ['tracking-page-lots', search],
    () => getLots(search),
    {
      keepPreviousData: true,
    }
  );

  let TrackingData: CardModelProps[] = [];

  if (isLoadingLots) {
    return (
      <div className="w-full h-full flex justify-center items-center">
        <Puff
          height="150"
          width="150"
          radius={1}
          color="#3b82f6"
          visible={true}
        />
      </div>
    );
  } else if (isErrorLots) {
    return (
      <div>
        <h1 className="font-semibold text-red-600">{errorLots?.message}</h1>
      </div>
    );
  } else {
    lotsResult?.results.forEach((lot) => {
      TrackingData.push({
        imageSrc:
          'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQ5pi71WjxZkW385n_7hBhTIv-sdgnUGjBZ10biXjH5cbUvSjfQ',
        modelName: lot.nomeModelo,
        amount: lot.lot.lotSize,
        stageModelName: lot.nomeEtapa,
        clientName: lot.nomeCliente,
        modelColor: lot.nomeCor,
        lotId: lot.lot.id,
        modelSize: lot.nomeTamanho.toString(),
        icon: TrackingIcons.WatingList,
        action: () => handleVisibilityEdit(true),
      });
    });
  }

  return (
    <div>
      <div className="container mx-auto py-5">
        <div>
          <div className="flex flex-col lg:flex-row justify-between m-5">
            <div className="flex overflow-x-auto">
              <div className="flex align-middle text-center mr-10 mb-5 items-center justify-center">
                <label className="truncate block mb-2 mr-3 text-medium font-medium text-gray-800">
                  All
                </label>

                <span className="truncate text-xs flex items-center justify-center font-semibold w-10 h-10 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
                  5
                </span>
              </div>

              <div className="flex align-middle text-center mr-10 mb-5 items-center justify-center">
                <label className="truncate block mb-2 mr-3 text-medium font-medium text-gray-800">
                  Wating List
                </label>

                <span className="truncate text-xs flex items-center justify-center font-semibold w-10 h-10 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
                  1
                </span>
              </div>

              <div className="flex align-middle text-center mr-10 mb-5 items-center justify-center">
                <label className="truncate block mb-2 mr-3 text-medium font-medium text-gray-800">
                  In Progress
                </label>

                <span className="truncate text-xs flex items-center justify-center font-semibold w-10 h-10 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
                  1
                </span>
              </div>

              <div className="flex align-middle text-center mr-10 mb-5 items-center justify-center">
                <label className="truncate block mb-2 mr-3 text-medium font-medium text-gray-800">
                  Shipping
                </label>

                <span className="truncate text-xs flex items-center justify-center font-semibold w-10 h-10 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
                  1
                </span>
              </div>

              <div className="flex align-middle text-center mr-10 mb-5 items-center justify-center">
                <label className="truncate block mb-2 mr-3 text-medium font-medium text-gray-800">
                  Finished
                </label>

                <span className="truncate text-xs flex items-center justify-center font-semibold w-10 h-10 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
                  1
                </span>
              </div>

              <div className="flex align-middle text-center mr-10 mb-5 items-center justify-center">
                <label className="truncate block mb-2 mr-3 text-medium font-medium text-gray-800">
                  Canceled
                </label>

                <span className="truncate text-xs flex items-center justify-center font-semibold w-10 h-10 py-1 px-2 uppercase rounded text-black bg-white last:mr-0 mr-1">
                  1
                </span>
              </div>
            </div>
            <div className="flex gap-5 lg:pl-2 pb-5">
              <Button
                onClick={() => handleVisibilityAdd(true)}
                color="dark"
                className="!bg-white !hover:bg-white !text-black w-10 h-10"
              >
                <AiOutlinePlus />
              </Button>
              <Button
                color="dark"
                className=" !bg-white !hover:bg-white !text-black !font-bold w-25 h-10"
                onClick={() => setIsFiltersVisible((prev) => !prev)}
              >
                <IoOptionsOutline size={20} className="mr-2" />
                <span className="text-xs">More</span>
              </Button>
            </div>
          </div>

          <div>
            {isFiltersVisible && (
              <div className="w-full grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-x-4 gap-y-2 p-4 my-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700">
                    Search
                  </label>
                  <div className="relative mt-1 rounded-md shadow-sm">
                    <input
                      type="text"
                      className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                      placeholder="Model name"
                      onChange={(e) => setSearch(e.target.value)}
                    />
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
        <div className="pt-5 m-5 grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-3 gap-4">
          {TrackingData.map((item, index) => (
            <CardModel
              key={index}
              imageSrc={item.imageSrc}
              modelName={item.modelName}
              amount={item.amount}
              stageModelName={item.stageModelName}
              clientName={item.clientName}
              modelColor={item.modelColor}
              modelSize={item.modelSize}
              lotId={item.lotId}
              icon={item.icon}
              action={() => handleVisibilityEdit(true)}
            />
          ))}
        </div>
      </div>
      <TrackingAdd open={openAdd} handleVisibility={handleVisibilityAdd} />
      <TrackingEdit open={openEdit} handleVisibility={handleVisibilityEdit} />
    </div>
  );
}
