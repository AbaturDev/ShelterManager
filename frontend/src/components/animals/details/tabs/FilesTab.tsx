import {
  Box,
  FileUpload,
  Flex,
  Heading,
  HStack,
  Icon,
  Stack,
  Text,
} from "@chakra-ui/react";
import { useState } from "react";
import { useAnimalFilesQuery } from "../../../../hooks/useAnimalFilesQuery";
import { LuUpload } from "react-icons/lu";
import { MdDelete } from "react-icons/md";
import { Loading, PaginationFooter } from "../../../commons";
import { useTranslation } from "react-i18next";
import { DeleteAnimalFileDialog } from "./DeleteAnimalFileDialog";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AnimalService } from "../../../../api/services/animals-service";
import { toaster } from "../../../ui/toaster";

interface FilesTabProps {
  animalId: string;
}

export const FilesTab = ({ animalId }: FilesTabProps) => {
  const { t } = useTranslation();
  const queryClient = useQueryClient();

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [isDeleteOpen, setIsDeleteOpen] = useState(false);
  const [deleteFileName, setDeleteFileName] = useState<string | null>(null);

  const uploadMutation = useMutation({
    mutationFn: (file: File) => AnimalService.uploadAnimalFile(animalId, file),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: ["animals", animalId, "files"],
      });
      toaster.create({
        type: "success",
        title: t("success"),
        description: t("animals.details.files.toast.upload.success"),
        closable: true,
      });
    },
    onError: () => {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.details.files.toast.upload.error"),
        closable: true,
      });
    },
  });

  const { data, isLoading, error } = useAnimalFilesQuery(animalId, {
    page: page,
    pageSize: pageSize,
  });

  if (isLoading) return <Loading />;
  if (error || data === undefined)
    return <Text color="red">{t("animals.details.files.error")}</Text>;

  const files = data?.items ?? [];

  const handleFileDownload = async (fileName: string) => {
    try {
      const blob = await AnimalService.getAnimalBlobFile(animalId, fileName);
      const url = window.URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = url;
      link.download = fileName;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

      window.URL.revokeObjectURL(url);
    } catch (error) {
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("animals.details.files.toast.download.error"),
        closable: true,
      });
    }
  };

  return (
    <>
      <FileUpload.Root
        w="full"
        alignItems="stretch"
        maxFiles={1}
        onFileChange={(uploadFiles) => {
          if (uploadFiles.acceptedFiles) {
            const fileToUpload = uploadFiles.acceptedFiles[0];
            const fileExists = files.some((f) => f.name === fileToUpload.name);
            if (fileExists) {
              toaster.create({
                type: "error",
                title: t("error"),
                description: t("animals.details.files.toast.upload.duplicate"),
                closable: true,
              });
              return;
            }

            uploadMutation.mutate(fileToUpload);
          }
        }}
      >
        <FileUpload.HiddenInput />
        <FileUpload.Dropzone>
          <Icon size="md" color="fg.muted">
            <LuUpload />
          </Icon>
          <FileUpload.DropzoneContent>
            <Box>{t("animals.details.files.dropzone")}</Box>
            <FileUpload.List />
          </FileUpload.DropzoneContent>
        </FileUpload.Dropzone>
      </FileUpload.Root>
      <Heading py={4}>{t("animals.details.files.title")}</Heading>

      <Stack>
        {files.map((file, index) => (
          <HStack
            key={`${file.name}-${index}`}
            justify="space-between"
            p={3}
            borderWidth="1px"
            borderRadius="md"
            _hover={{ bg: "gray.50" }}
          >
            <Text
              color="blue.600"
              fontSize="md"
              fontWeight={"semibold"}
              _hover={{ textDecoration: "underline" }}
              cursor="pointer"
              onClick={async () => await handleFileDownload(file.name)}
            >
              {file.name}
            </Text>
            <Icon
              as={MdDelete}
              boxSize={6}
              _hover={{ cursor: "pointer" }}
              onClick={() => {
                setDeleteFileName(file.name);
                setIsDeleteOpen(true);
              }}
            />
          </HStack>
        ))}
        <Flex w="100%" align={"center"} justify={"center"}>
          <PaginationFooter
            page={page}
            pageSize={pageSize}
            onPageChange={setPage}
            onPageSizeChange={setPageSize}
            totalItemsCount={data?.totalItemsCount!}
          />
        </Flex>
      </Stack>
      {deleteFileName && (
        <DeleteAnimalFileDialog
          id={animalId}
          isOpen={isDeleteOpen}
          fileName={deleteFileName}
          onClose={() => {
            setIsDeleteOpen(false);
            setDeleteFileName(null);
          }}
          onSuccess={() => {
            setIsDeleteOpen(false);
            setDeleteFileName(null);
          }}
        />
      )}
    </>
  );
};
