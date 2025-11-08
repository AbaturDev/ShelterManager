import {
  Box,
  Button,
  Flex,
  Heading,
  HStack,
  Icon,
  Link,
  Separator,
  SimpleGrid,
  Stack,
  Text,
} from "@chakra-ui/react";
import { MdDateRange, MdPerson } from "react-icons/md";
import type { AdoptionDetails } from "../../../models/adoptions";
import { useTranslation } from "react-i18next";
import { Loading } from "../../commons";
import { AdoptionStatusBadge } from "../AdoptionStatusBadge";
import { CiStickyNote } from "react-icons/ci";
import { FaRegFilePdf } from "react-icons/fa";
import { AdoptionService } from "../../../api/services/adoption-service";
import { toaster } from "../../ui/toaster";
import { useState } from "react";
import i18n from "../../../i18n/i18n";

interface Props {
  error?: Error | null;
  adoption: AdoptionDetails | undefined;
  isLoading: boolean;
}

export const AdoptionDetailsCard = ({ error, adoption, isLoading }: Props) => {
  const { t } = useTranslation();

  const [isPdfLoading, setIsPdfLoading] = useState(false);

  if (isLoading) return <Loading />;
  if (error || adoption === undefined)
    return <Text color="red">{t("adoptions.details.error")}</Text>;

  const currentLang = i18n.language;

  const generatePdfAgreement = async () => {
    try {
      setIsPdfLoading(true);

      const blob = await AdoptionService.getAdoptionAgreement(
        adoption.id,
        currentLang
      );
      const url = window.URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = url;
      link.download = `adoption_agreement_${adoption.animalName}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);

      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.log(error);
      toaster.create({
        type: "error",
        title: t("error"),
        description: t("adoptions.details.pdfError"),
        closable: true,
      });
    } finally {
      setIsPdfLoading(false);
    }
  };

  return (
    <Box maxW="800px" mx="auto" p={8} bg="white" rounded="2xl" shadow="md">
      <Stack>
        <Flex justify="space-between" align="center">
          <HStack>
            <Heading>{t("adoptions.details.title")}</Heading>
            <Heading>
              <Link
                variant={"underline"}
                href={`/animals/${adoption.animalId}`}
              >
                {adoption.animalName}
              </Link>
            </Heading>
          </HStack>
          <AdoptionStatusBadge status={adoption.status} />
        </Flex>

        <HStack color="gray.600" fontSize="sm" justifyContent={"space-between"}>
          <HStack>
            <Icon as={MdDateRange} />
            <Text>
              {t("adoptions.details.start")}:{" "}
              {new Date(adoption.startAdoptionProcess).toLocaleDateString()}
            </Text>
          </HStack>
          {adoption.adoptionDate && (
            <HStack>
              <Icon as={MdDateRange} />
              <Text>
                {t("adoptions.details.date")}:{" "}
                {new Date(adoption.adoptionDate).toLocaleDateString()}
              </Text>
            </HStack>
          )}
        </HStack>

        <Separator marginTop={3} />

        <Box>
          <Heading size="lg" mb={4}>
            <HStack>
              <Icon as={CiStickyNote} />
              <Text>{t("adoptions.details.note")}</Text>
            </HStack>
          </Heading>

          {adoption.note ? (
            <Text whiteSpace="pre-wrap">{adoption.note}</Text>
          ) : (
            <Text color="gray.400">{t("adoptions.details.noNote")}</Text>
          )}
        </Box>

        <Separator marginTop={3} />

        <Box>
          <Heading size="lg" mb={4}>
            <HStack>
              <Icon as={MdPerson} />
              <Text>{t("adoptions.details.person.info")}</Text>
            </HStack>
          </Heading>
          <SimpleGrid columns={{ base: 1, md: 2 }}>
            <Text>
              <b>{t("adoptions.details.person.name")}:</b>{" "}
              {adoption.person.name} {adoption.person.surname}
            </Text>
            <Text>
              <b>{t("adoptions.details.person.email")}:</b>{" "}
              {adoption.person.email}
            </Text>
            <Text>
              <b>{t("adoptions.details.person.phoneNumber")}:</b>{" "}
              {adoption.person.phoneNumber}
            </Text>
            <Text>
              <b>{t("adoptions.details.person.pesel")}:</b>{" "}
              {adoption.person.pesel}
            </Text>
            <Text>
              <b>{t("adoptions.details.person.documentId")}:</b>{" "}
              {adoption.person.documentId}
            </Text>
            <Text>
              <b>{t("adoptions.details.person.address")}:</b>{" "}
              {adoption.person.street}, {adoption.person.city},{" "}
              {adoption.person.postalCode}
            </Text>
          </SimpleGrid>
        </Box>

        <Separator marginTop={5} />

        <Flex
          w="100%"
          justifyContent={"space-between"}
          color="gray.500"
          fontSize="sm"
        >
          <Text>
            {t("adoptions.details.created")}:{" "}
            {new Date(adoption.createdAt).toLocaleString()}
          </Text>
          <Text>
            {t("adoptions.details.updated")}:{" "}
            {new Date(adoption.updatedAt).toLocaleString()}
          </Text>
        </Flex>
        <Flex w="100%" justifyContent={"center"} marginTop={3}>
          <Button
            loading={isPdfLoading}
            size={"lg"}
            background={"green.400"}
            onClick={async () => await generatePdfAgreement()}
          >
            <Icon as={FaRegFilePdf} />
            {t("adoptions.details.pdf")}
          </Button>
        </Flex>
      </Stack>
    </Box>
  );
};
