namespace JobMagnet.Application.UseCases.CvParser.Commands;

public record CvParserCommand(Stream Stream, string FileName, string FileExtension);