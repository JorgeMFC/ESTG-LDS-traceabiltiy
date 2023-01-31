import { useState } from 'react';
import { MdOutlineBlock } from 'react-icons/md';
import { IoMdUnlock } from 'react-icons/io';
import DialogRemove from '../../../components/DialogRemove';

import Table from '../../../components/Table';
import { TableSizes } from '../../../models/TableSizes';

export default function OrganizationsList() {
  const [openBlock, setOpenBlock] = useState(false);
  const handleVisibilityBlock = (visibility: boolean) => {
    setOpenBlock(visibility);
  };

  const [openUnblock, setOpenUnblock] = useState(false);
  const handleVisibilityUnblock = (visibility: boolean) => {
    setOpenUnblock(visibility);
  };

  const [page, setPage] = useState(1);
  let maxPage = 3;

  const previousPage = () => {
    let aux = page;
    if (aux > 1) {
      setPage(aux - 1);
    }
  };

  const nextPage = () => {
    let aux = page;
    if (aux < maxPage) {
      setPage(aux + 1);
    }
  };

  const title = ['Name', 'Email', 'Date', 'Owner', ''];
  const content = [
    [
      'Org1',
      'org1@example.com',
      '15/03/2022',
      'Owner1',
      <IoMdUnlock
        className="cursor-pointer"
        onClick={() => handleVisibilityUnblock(true)}
      />,
    ],
    [
      'Org2',
      'org2@example.com',
      '22/10/2021',
      'Owner2',
      <MdOutlineBlock
        className="cursor-pointer"
        onClick={() => handleVisibilityBlock(true)}
      />,
    ],
    [
      'Org3',
      'org3@example.com',
      '05/08/2021',
      'Owner3',
      <MdOutlineBlock
        className="cursor-pointer"
        onClick={() => handleVisibilityBlock(true)}
      />,
    ],
  ];

  return (
    <div className="flex justify-center">
      <Table
        size={TableSizes.SMALL}
        titles={title}
        content={content}
        page={page}
        maxPage={maxPage}
        nextPage={nextPage}
        previousPage={previousPage}
        setSearch={(search: string) => {
          console.log(search);
        }}
        placeholder="Search Organizations by Name, Email, Date or Owner"
        changeNumberRows={(value) => {
          console.log(value);
        }}
        filters={
          <>
            <div>
              <label className="block text-sm font-medium text-gray-700">
                Initial Date
              </label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <input
                  type="date"
                  className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Initial Date"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                End Date
              </label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <input
                  type="date"
                  className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="End Date"
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700">
                Owner
              </label>
              <div className="relative mt-1 rounded-md shadow-sm">
                <input
                  type="text"
                  className="block w-full rounded-md border-gray-300 focus:border-indigo-500 focus:ring-indigo-500 sm:text-sm"
                  placeholder="Owner"
                />
              </div>
            </div>
          </>
        }
      />

      <DialogRemove
        message="Are you sure you want to unblock Org1?"
        open={openUnblock}
        handleVisibility={handleVisibilityUnblock}
        actionButtonYes={() => handleVisibilityUnblock(false)}
        actionButtonCancel={() => handleVisibilityUnblock(false)}
      />
      <DialogRemove
        message="Are you sure you want to block Org2?"
        open={openBlock}
        handleVisibility={handleVisibilityBlock}
        actionButtonYes={() => handleVisibilityBlock(false)}
        actionButtonCancel={() => handleVisibilityBlock(false)}
      />
    </div>
  );
}
