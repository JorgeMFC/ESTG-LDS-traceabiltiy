import { Button } from 'flowbite-react/lib/esm/components/Button';
import DefaultAvatar from '../../../assets/default_avatar.jpg';
import { useRef, useState, useEffect, useContext } from 'react';
import { TbEdit } from 'react-icons/tb';
import { User } from '../../../models/api/UserModel';

export default function SettingsList() {
  const [file, setFile] = useState<any>();
  const inputFile = useRef<HTMLInputElement>(null);

  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {}, []);

  const handleClick = () => {
    inputFile.current?.click();
  };

  function handleChange1(event: any) {
    setFile(URL.createObjectURL(event.target.files[0]));
    console.log(file);
  }
  return (
    <div className="mx-auto rounded min-h-full flex justify-center">
      <div className="w-11/12 sm:3/4 bg-white my-8 p-8 rounded-xl shadow">
        <h1 className="text-xl sm:text-2xl font-bold text-gray-600">
          Profile Details
        </h1>

        <div className="flex items-center justify-center flex-wrap mt-10">
          <div className="w-full sm:w-1/4">
            <div className="flex justify-center mx-auto py-5">
              <div
                className="overflow-hiddenw-2/4 aspect-video cursor-pointer rounded-full relative group"
                onClick={handleClick}
              >
                <div className="w-full h-full rounded-full opacity-0 group-hover:opacity-40 transition duration-300 ease-in-out cursor-pointer absolute bg-black  text-white">
                  <div className="w-full h-full text-xl group-hover:opacity-100 group-hover:translate-y-0 translate-y-4 transform transition duration-300 ease-in-out">
                    <div className="flex w-full h-full font-bold justify-center items-center">
                      <TbEdit size={60} />
                    </div>
                  </div>
                </div>

                <img
                  className="cursor-pointer w-32 h-32 md:w-44 md:h-44 rounded-full mx-auto object-cover"
                  src={file ? file : DefaultAvatar}
                />
                <input
                  type="file"
                  className="hidden"
                  onChange={handleChange1}
                  ref={inputFile}
                />
              </div>
            </div>
          </div>

          <div className="flex flex-col gap-4 sm:gap-8 w-3/4 mt-8 sm:mt-0">
            <div className="flex flex-col sm:flex-row justify-between w-full sm:px-20 gap-4">
              <div className="w-full">
                <label className="block text-sm font-medium text-gray-700">
                  Name *
                </label>
                <input
                  type="text"
                  className="w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Name"
                  defaultValue="Erin Levin"
                />
              </div>

              <div className="w-full">
                <label className="block text-sm font-medium text-gray-700">
                  Email *
                </label>
                <input
                  type="text"
                  className="w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Email"
                  defaultValue="erin@example.com"
                />
              </div>
            </div>

            <div className="flex flex-col sm:flex-row justify-between w-full sm:px-20 gap-4">
              <div className="w-full">
                <label className="block text-sm font-medium text-gray-700">
                  Wallet *
                </label>
                <input
                  type="text"
                  className="disabled:bg-slate-50 disabled:text-slate-500 disabled:border-slate-200 disabled:shadow-none w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Wallet"
                  defaultValue="0xE2A41ED3407DEBE442FF80CD07B8H5CJKL23"
                  disabled
                />
              </div>

              <div className="w-full">
                <label className="block text-sm font-medium text-gray-700">
                  Birth Date
                </label>
                <input
                  type="text"
                  className="w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Birth Date"
                  defaultValue="12/12/1999"
                />
              </div>
            </div>
          </div>
        </div>

        <div className="flex flex-col relative mt-10 ">
          <div className="w-12 sm:w-auto">
            <h1 className="text-xl sm:text-2xl font-bold text-gray-600">
              Organization Details
            </h1>
          </div>

          <div className="flex items-center absolute top-0 right-0">
            <img
              className="w-12 h-12 sm:w-16 sm:h-16 object-cover rounded-lg"
              src="https://img1.wsimg.com/isteam/ip/eed91d6b-838b-4d04-b670-9f886e21dc70/logomini2.png"
              alt=""
            />
            <h1 className="font-bold text-lg">Coincal LDA</h1>
          </div>

          <div className="grid grid-cols-4 mt-10 gap-2">
            <div className="col-span-4 sm:col-span-1">
              <img
                className="rounded-lg w-3/4 sm:w-full lg:w-3/4 mx-auto"
                src="https://images.adsttc.com/media/images/584e/4b0f/e58e/ce89/a700/018a/large_jpg/130421001.jpg?1481526017"
                alt=""
              />
            </div>
            <div className="col-span-4 sm:col-span-3 flex flex-col justify-center gap-7 sm:px-10 sm:ml-10">
              <div className="w-full flex flex-col justify-center px-12 sm:px-0">
                <label className="block text-sm font-medium text-gray-700">
                  Name
                </label>
                <input
                  type="text"
                  className="disabled:bg-slate-50 disabled:text-slate-500 disabled:border-slate-200 disabled:shadow-none block sm:w-3/4 rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Organization Name"
                  defaultValue="Organization 1"
                  disabled
                />
              </div>

              <div className="w-full flex flex-col justify-center px-12 sm:px-0">
                <label className="block text-sm font-medium text-gray-700">
                  Email
                </label>
                <input
                  type="text"
                  className="disabled:bg-slate-50 disabled:text-slate-500 disabled:border-slate-200 disabled:shadow-none block sm:w-3/4 rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Organization Email"
                  defaultValue="organization1@example.com"
                  disabled
                />
              </div>
            </div>
          </div>
        </div>

        <div className="flex justify-end">
          <Button color="dark" size="md" className="!h-full">
            Save
          </Button>
        </div>
      </div>
    </div>
  );
}
