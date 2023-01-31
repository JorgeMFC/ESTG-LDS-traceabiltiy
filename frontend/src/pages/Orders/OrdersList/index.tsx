import { useState } from 'react';
import { Button } from 'flowbite-react';

import OrdersSee from '../OrdersSee';
import { TrackingIcons } from '../../../models/TrackingIcons';
import { IoOptionsOutline } from 'react-icons/io5';
import CardModel from '../../../components/CardModel';

export default function OrdersList() {
  const [openSee, setOpenSee] = useState(false);
  const handleVisibilitySee = (visibility: boolean) => {
    setOpenSee(visibility);
  };

  const [openEdit, setOpenEdit] = useState(false);
  const handleVisibilityEdit = (visibility: boolean) => {
    setOpenEdit(visibility);
  };

  const TrackingData = [
    {
      id: '1',
      imageSrc:
        'https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcQ5pi71WjxZkW385n_7hBhTIv-sdgnUGjBZ10biXjH5cbUvSjfQ',
      title: 'Nike Air Max 90',
      phase: 'Colagem',
      timeLeft: '1 Week Left',
      progress: 90,
      icon: TrackingIcons.WatingList,
    },
    {
      id: '2',
      imageSrc:
        'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR-3SWjRH5oJPBqbsGAx-OJ2mnRq3ubrNq4KDErQzUkSWMwI2Cg',
      title: 'Nike Air Max 90',
      phase: 'Colagem',
      timeLeft: '1 Week Left',
      progress: 90,
      icon: TrackingIcons.InProgress,
    },
    {
      id: '3',
      imageSrc:
        'https://www.ginova.pt/26823-large_default/sapato-casual-formal-homem-ref64170p.jpg',
      title: 'Nike Air Max 90',
      phase: 'Colagem',
      timeLeft: '1 Week Left',
      progress: 90,
      icon: TrackingIcons.Shipping,
    },
    {
      id: '4',
      imageSrc:
        'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcThO_uJoza_t4Wt2VAzIv2fxXA9qBIW-ImUd4UZACayvx773Kb5',
      title: 'Nike Air Max 90',
      phase: 'Colagem',
      timeLeft: '1 Week Left',
      progress: 90,
      icon: TrackingIcons.Canceled,
    },
    {
      id: '5',
      imageSrc:
        'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcR7ATa6f5GsTeVCjKYmiD7TF9C8IE9qeC8g6NY2XPxxqtvlZ2i7',
      title: 'Nike Air Max 90',
      phase: 'Colagem',
      timeLeft: '1 Week Left',
      progress: 90,
      icon: TrackingIcons.Finished,
    },
  ];

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
                color="dark"
                className=" !bg-white !hover:bg-white !text-black !font-bold w-25 h-10"
              >
                <IoOptionsOutline size={20} className="mr-2" />
                <span className="text-xs">More</span>
              </Button>
            </div>
          </div>
        </div>
        <div className="pt-5 m-5 grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
          {/* {TrackingData.map((item, index) => (
            <CardModel
              key={index}
              imageSrc={item.imageSrc}
              modelName={item.title}
              phase={item.phase}
              timeLeft={item.timeLeft}
              progress={item.progress}
              icon={item.icon}
              action={() => handleVisibilityEdit(true)}
            />
          ))} */}
        </div>
      </div>
      <OrdersSee open={openEdit} handleVisibility={handleVisibilityEdit} />
    </div>
  );
}
